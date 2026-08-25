const { execSync } = require('child_process');
const path = require('path');
const fs = require('fs');
const https = require('https');

const videoDir = 'C:\\Users\\gilso\\OneDrive\\Área de Trabalho\\Gilson DOCS\\projeto-portfolio\\video-portfolio';
const desktopVid = path.join(videoDir, '0819(1).mp4');
const mobileVid = path.join(videoDir, '0820.mp4');

const desktopFinal = path.join(videoDir, 'Portfolio_Desktop_Final.mp4');
const mobileFinal = path.join(videoDir, 'Portfolio_Mobile_Final.mp4');
const musicFile = path.join(videoDir, 'bg_music.mp3');
const FFMPEG = require('ffmpeg-static');

console.log('Iniciando edição dos vídeos...');

async function downloadMusic(url, dest) {
  return new Promise((resolve, reject) => {
    if (fs.existsSync(dest)) return resolve();
    console.log('Baixando música de fundo...');
    const file = fs.createWriteStream(dest);
    https.get(url, (response) => {
      response.pipe(file);
      file.on('finish', () => {
        file.close(resolve);
      });
    }).on('error', (err) => {
      fs.unlink(dest, () => {});
      reject(err);
    });
  });
}

async function main() {
  try {
    // 1. Download royalty free track
    await downloadMusic('https://www.soundhelix.com/examples/mp3/SoundHelix-Song-1.mp3', musicFile);
    console.log('Música de fundo baixada com sucesso.');

    // 2. Process Mobile Video (Cut at 48 seconds to remove Curriculum, remove pauses, add music)
    console.log('Editando versão Mobile (0820.mp4)...');
    const mobileTrimmed = path.join(videoDir, 'mobile_trimmed.mp4');
    
    // Trim to 48s, remove visual pauses (mpdecimate), remove original audio (-an)
    const mobileFilter = `"mpdecimate,setpts=N/FRAME_RATE/TB"`;
    execSync(`"${FFMPEG}" -y -t 48 -i "${mobileVid}" -vf ${mobileFilter} -an -c:v libx264 -preset fast "${mobileTrimmed}"`, { stdio: 'inherit' });
    
    // Merge with music and fade out audio at the end
    // First, find the new duration of the trimmed & decimated video
    const mobileDurOutput = execSync(`ffprobe -v error -show_entries format=duration -of default=noprint_wrappers=1:nokey=1 "${mobileTrimmed}"`).toString().trim();
    const mobileDur = parseFloat(mobileDurOutput);
    console.log(`Duração do Mobile Editado: ${mobileDur} segundos`);
    
    // Mix music: -stream_loop -1 loops the music if it's too short, -t ensures it stops at video end
    // afade fades out the last 2 seconds
    execSync(`"${FFMPEG}" -y -i "${mobileTrimmed}" -stream_loop -1 -i "${musicFile}" -c:v copy -c:a aac -b:a 128k -af "afade=t=out:st=${Math.max(0, mobileDur - 2)}:d=2,volume=0.5" -shortest "${mobileFinal}"`, { stdio: 'inherit' });
    
    fs.unlinkSync(mobileTrimmed);
    console.log('=> Versão Mobile concluída!');

    // 3. Process Desktop Video (Remove pauses, add music)
    console.log('\\nEditando versão Desktop (0819(1).mp4)...');
    const desktopTrimmed = path.join(videoDir, 'desktop_trimmed.mp4');
    
    // Remove visual pauses, remove original audio (-an)
    execSync(`"${FFMPEG}" -y -i "${desktopVid}" -vf ${mobileFilter} -an -c:v libx264 -preset fast "${desktopTrimmed}"`, { stdio: 'inherit' });
    
    const desktopDurOutput = execSync(`ffprobe -v error -show_entries format=duration -of default=noprint_wrappers=1:nokey=1 "${desktopTrimmed}"`).toString().trim();
    const desktopDur = parseFloat(desktopDurOutput);
    console.log(`Duração do Desktop Editado: ${desktopDur} segundos`);
    
    execSync(`"${FFMPEG}" -y -i "${desktopTrimmed}" -stream_loop -1 -i "${musicFile}" -c:v copy -c:a aac -b:a 128k -af "afade=t=out:st=${Math.max(0, desktopDur - 2)}:d=2,volume=0.5" -shortest "${desktopFinal}"`, { stdio: 'inherit' });
    
    fs.unlinkSync(desktopTrimmed);
    console.log('=> Versão Desktop concluída!');
    
    console.log('\\nTodos os vídeos editados com sucesso!');
    console.log('- Portfolio_Mobile_Final.mp4');
    console.log('- Portfolio_Desktop_Final.mp4');
    
  } catch (err) {
    console.error('Erro ao editar vídeos:', err.message);
  }
}

main();
