/**
 * Portfolio Video Recorder v3
 * Architecture: Concurrent action + screenshots with robust fallback
 */

const puppeteer = require('puppeteer');
const fs = require('fs');
const path = require('path');
const https = require('https');
const http = require('http');
const { execSync } = require('child_process');

const SITE_URL = 'http://localhost:4200/';
const OUTPUT_DIR = 'C:\\Users\\gilso\\OneDrive\\Área de Trabalho\\Gilson DOCS\\projeto-portfolio\\video-portfolio';
const TEMP_DIR = path.join(__dirname, 'temp');
const FFMPEG = require('ffmpeg-static');
const ADMIN_EMAIL = 'gilsonsouza.dev@gmail.com';
const FPS = 10;

if (!fs.existsSync(TEMP_DIR)) fs.mkdirSync(TEMP_DIR, { recursive: true });
if (!fs.existsSync(OUTPUT_DIR)) fs.mkdirSync(OUTPUT_DIR, { recursive: true });

const sleep = (ms) => new Promise(r => setTimeout(r, ms));

const SCENES = [
  { id: 'abertura',  text: 'Olá! Bem-vindo ao portfólio de Gilson Souza, Desenvolvedor Full Stack. Este projeto foi totalmente desenvolvido em Angular no frontend e ponto NET no backend.', action: 'hero', url: SITE_URL },
  { id: 'sobre',     text: 'Na seção Sobre Mim, você encontra um resumo sobre minha trajetória, paixão por tecnologia e minha constante busca por conhecimento e inovação.', action: 'about', url: SITE_URL },
  { id: 'resumo',    text: 'Como profissional focado em Cloud e melhores práticas, destaco aqui as minhas certificações pela AWS. Também apresento minhas principais habilidades técnicas no ecossistema web.', action: 'resume', url: SITE_URL },
  { id: 'portfolio', text: 'Na área de portfólio, desenvolvi sistemas robustos. Destacam-se o painel administrativo para e-commerce, o CRM corporativo e soluções logísticas, sempre com foco em escalabilidade e design limpo.', action: 'portfolio', url: SITE_URL },
  { id: 'mobile',    text: 'Todo o portfólio é perfeitamente responsivo. O menu mobile foi construído para ser intuitivo e premium em qualquer dispositivo.', action: 'mobile', url: SITE_URL },
  { id: 'contato',   text: 'A seção de contato conta com integração completa via serviço de email no backend. Ao enviar a mensagem, recebo instantaneamente na minha caixa de entrada.', action: 'contact', url: SITE_URL },
  { id: 'admin',     text: 'Para gerenciar o portfólio, construí uma área administrativa segura, acessada sem senhas estáticas, utilizando autenticação por código dinâmico enviado diretamente ao meu e-mail.', action: 'admin', url: SITE_URL + 'auth' },
  { id: 'edicao',    text: 'Uma vez autenticado, o modo de edição permite alterar textos diretamente na tela, adicionar projetos, gerenciar habilidades e fazer upload de novas imagens, com tudo sendo salvo no banco de dados.', action: 'edit', url: SITE_URL },
  { id: 'fim',       text: 'Este portfólio demonstra habilidades técnicas e atenção a detalhes, desde a arquitetura de backend até a experiência do usuário. Obrigado por assistir!', action: 'end', url: SITE_URL }
];

async function scrollTo(page, y) {
  try {
    const steps = 15;
    for (let i = 1; i <= steps; i++) {
      await page.evaluate((target, step, total) => {
        window.scrollTo(0, window.scrollY + (target - window.scrollY) * (step / total));
      }, y, i, steps);
      await sleep(60);
    }
  } catch (e) {}
}

async function moveMouse(page, x1, y1, x2, y2) {
  const steps = 15;
  for (let i = 1; i <= steps; i++) {
    try {
      await page.mouse.move(x1 + ((x2 - x1) * i) / steps, y1 + ((y2 - y1) * i) / steps);
      await sleep(40);
    } catch (e) {}
  }
}

async function humanType(page, selector, text) {
  try {
    await page.waitForSelector(selector, { timeout: 3000 });
    await page.click(selector);
    await sleep(300);
    await page.keyboard.type(text, { delay: 70 });
  } catch (e) {}
}

async function runSceneAction(page, action) {
  if (action === 'hero') {
    await sleep(2000);
    await moveMouse(page, 640, 400, 400, 280);
    await sleep(800);
    await moveMouse(page, 400, 280, 700, 220);
  }
  if (action === 'about') {
    await sleep(1500);
    await scrollTo(page, 650);
    await sleep(500);
    await moveMouse(page, 300, 500, 600, 450);
  }
  if (action === 'resume') {
    await sleep(1500);
    await scrollTo(page, 1400);
    await sleep(800);
    await moveMouse(page, 400, 500, 300, 560);
    await sleep(500);
    await moveMouse(page, 300, 560, 700, 560);
  }
  if (action === 'portfolio') {
    await sleep(1500);
    await scrollTo(page, 2200);
    await sleep(800);
    await moveMouse(page, 300, 500, 700, 500);
    await sleep(600);
    try {
      const btns = await page.$$('.carousel-next, [aria-label="Next"], .next');
      if (btns.length > 0) {
        await btns[0].click(); await sleep(900);
        await btns[0].click();
      }
    } catch (e) {}
  }
  if (action === 'mobile') {
    await sleep(1500);
    try {
      const toggle = await page.$('.menu-toggle');
      if (toggle) {
        await toggle.click();
        await sleep(2000);
        const closeBtn = await page.$('.drawer-close');
        if (closeBtn) { await closeBtn.click(); }
      }
    } catch (e) {}
  }
  if (action === 'contact') {
    await sleep(1500);
    await scrollTo(page, 9999);
    await sleep(1000);
    await humanType(page, 'input[placeholder*="ome"], input[name="name"]', 'Recrutador Tech');
    await humanType(page, 'input[type="email"]', 'recrutador@empresa.com.br');
    await humanType(page, 'textarea', 'Olá Gilson! Adorei seu portfólio. Temos uma proposta incrível para você!');
    await sleep(500);
    try {
      const send = await page.$('button[type="submit"]');
      if (send) { await moveMouse(page, 640, 700, 400, 750); await sleep(300); await send.click(); }
    } catch (e) {}
  }
  if (action === 'admin') {
    await sleep(2000);
    await humanType(page, 'input[type="email"]', ADMIN_EMAIL);
    await sleep(800);
    try {
      const btn = await page.$('button[type="submit"]');
      if (btn) {
        await moveMouse(page, 400, 400, 640, 500);
        await btn.click();
        await sleep(2000);
      }
    } catch (e) {}
    await humanType(page, 'input[type="text"]', '123456');
    await sleep(800);
    try {
      const verifyBtn = await page.$('button[type="submit"]');
      if (verifyBtn) {
        await moveMouse(page, 640, 500, 640, 600);
        await verifyBtn.click();
        await sleep(3000);
      }
    } catch (e) {}
  }
  if (action === 'edit') {
    await sleep(1500);
    // Find an edit button (adjust selector as needed)
    try {
      const editBtn = await page.$('.edit-button');
      if (editBtn) {
        await editBtn.click();
        await sleep(1000);
      }
    } catch(e) {}
    await scrollTo(page, 600);
    await sleep(600);
    await moveMouse(page, 200, 400, 500, 380);
    // Try to type in an input if one exists
    try {
      const input = await page.$('input, textarea');
      if (input) {
        await input.focus();
        await page.keyboard.type(' (Atualizado)');
        await sleep(500);
      }
      const saveBtn = await page.$('.btn-save');
      if (saveBtn) {
        await saveBtn.click();
      }
    } catch(e) {}
    await sleep(700);
    await scrollTo(page, 400);
    await sleep(600);
  }
  if (action === 'end') {
    await sleep(1000);
    await moveMouse(page, 640, 400, 640, 300);
  }
}

async function captureConcurrent(page, action, framesDir, durationSecs) {
  if (!fs.existsSync(framesDir)) fs.mkdirSync(framesDir, { recursive: true });
  const totalFrames = Math.ceil(durationSecs * FPS);
  const delay = Math.round(1000 / FPS);

  console.log(`  📸 Capturing ${totalFrames} frames...`);

  // Start action in background
  const actionPromise = runSceneAction(page, action);

  let lastSuccessfulFrame = null;

  // Capture frames in a loop
  for (let i = 0; i < totalFrames; i++) {
    const file = path.join(framesDir, `frame_${String(i).padStart(5, '0')}.png`);
    try {
      // Small timeout to not block the loop for long
      await page.screenshot({ path: file, type: 'png', timeout: 5000 });
      lastSuccessfulFrame = file;
    } catch (e) {
      // If error (navigating, disconnected), use previous frame
      if (lastSuccessfulFrame && fs.existsSync(lastSuccessfulFrame)) {
        fs.copyFileSync(lastSuccessfulFrame, file);
      }
    }
    if (i % 10 === 0) process.stdout.write(`  frame ${i}/${totalFrames}\r`);
    await sleep(delay);
  }

  // Ensure action finishes if it's still running
  await actionPromise.catch(() => {});
  console.log(`  ✅ ${totalFrames} frames captured.`);
}

async function downloadAudio(text, file) {
  if (fs.existsSync(file)) return;
  const googleTTS = require('google-tts-api');
  const urls = googleTTS.getAllAudioUrls(text, { lang: 'pt-BR', slow: false });
  const buffers = [];
  for (const { url } of urls) {
    const buf = await new Promise((resolve, reject) => {
      const client = url.startsWith('https') ? https : http;
      client.get(url, { headers: { 'User-Agent': 'Mozilla/5.0' } }, res => {
        const chunks = [];
        res.on('data', c => chunks.push(c));
        res.on('end', () => resolve(Buffer.concat(chunks)));
        res.on('error', reject);
      }).on('error', reject);
    });
    buffers.push(buf);
  }
  fs.writeFileSync(file, Buffer.concat(buffers));
}

function audioDuration(file) {
  try {
    execSync(`"${FFMPEG}" -i "${file}" 2>&1`, { encoding: 'utf8' });
  } catch (e) {
    const out = (e.stderr || '') + (e.stdout || '') + (e.message || '');
    const m = out.match(/Duration: (\d+):(\d+):([\d.]+)/);
    if (m) return +m[2] * 60 + parseFloat(m[3]);
  }
  return 6;
}

function encodeScene(framesDir, audioFile, outFile) {
  const pattern = path.join(framesDir, 'frame_%05d.png');
  execSync(`"${FFMPEG}" -y -framerate ${FPS} -i "${pattern}" -i "${audioFile}" -c:v libx264 -pix_fmt yuv420p -c:a aac -shortest -movflags +faststart "${outFile}"`, { stdio: 'pipe' });
}

function concatVideos(files, outFile) {
  const listPath = path.join(TEMP_DIR, 'list.txt');
  fs.writeFileSync(listPath, files.map(f => `file '${f.replace(/\\/g, '/')}'`).join('\n'));
  execSync(`"${FFMPEG}" -y -f concat -safe 0 -i "${listPath}" -c copy "${outFile}"`, { stdio: 'pipe' });
}

async function main() {
  console.log('🚀 Portfolio Video Recorder v3 starting...\n');

  // Launch fresh browser
  const browser = await puppeteer.launch({
    headless: false,
    protocolTimeout: 60000,
    defaultViewport: null,
    args: ['--window-size=1280,800', '--no-sandbox', '--disable-setuid-sandbox']
  });

  const page = await browser.newPage();

  await page.setRequestInterception(true);
  page.on('request', interceptedRequest => {
    const url = interceptedRequest.url();
    if (url.includes('/api/auth/request-link') && interceptedRequest.method() === 'POST') {
      interceptedRequest.respond({ status: 200, contentType: 'application/json', body: JSON.stringify({ message: 'Mock email sent' }) });
    } else if (url.includes('/api/auth/verify')) {
      interceptedRequest.respond({ status: 200, contentType: 'application/json', body: JSON.stringify({ sessionToken: 'mock-token', message: 'Mock Authenticated' }) });
    } else if (interceptedRequest.method() === 'PUT' || interceptedRequest.method() === 'POST' || interceptedRequest.method() === 'DELETE') {
      interceptedRequest.respond({ status: 200, contentType: 'application/json', body: JSON.stringify({ success: true, message: 'Saved successfully' }) });
    } else {
      interceptedRequest.continue();
    }
  });

  const sceneVideos = [];

  for (const scene of SCENES) {
    const sceneFile = path.join(TEMP_DIR, `scene_${scene.id}.mp4`);
    if (fs.existsSync(sceneFile)) {
      console.log(`\n⏭️  Skipping already completed scene: ${scene.id}`);
      sceneVideos.push(sceneFile);
      continue;
    }
    
    console.log(`\n🎬 Scene: ${scene.id}`);

    // Setup viewport
    if (scene.action === 'mobile') {
      await page.setViewport({ width: 390, height: 844 });
    } else {
      await page.setViewport({ width: 1280, height: 800 });
    }

    // 1. Audio
    const audioFile = path.join(TEMP_DIR, `audio_${scene.id}.mp3`);
    await downloadAudio(scene.text, audioFile);
    const dur = audioDuration(audioFile);
    console.log(`  ⏱️  Duration: ${dur.toFixed(1)}s`);

    // 2. Navigate FIRST before screenshots
    console.log(`  🧭 Navigating to: ${scene.url}`);
    await page.goto(scene.url, { waitUntil: 'networkidle2', timeout: 30000 }).catch(() => {});
    await sleep(1000);

    // 3. Capture frames CONCURRENTLY with actions
    const framesDir = path.join(TEMP_DIR, `frames_${scene.id}`);
    await captureConcurrent(page, scene.action, framesDir, dur + 1);

    // 4. Encode scene
    console.log(`  🔧 Encoding...`);
    encodeScene(framesDir, audioFile, sceneFile);
    sceneVideos.push(sceneFile);
  }

  await browser.close();

  console.log('\n🎞️  Concatenating all scenes...');
  const finalFile = path.join(OUTPUT_DIR, 'Portfolio_Apresentacao.mp4');
  concatVideos(sceneVideos, finalFile);

  console.log('\n✅✅✅  VIDEO COMPLETE! ✅✅✅');
  console.log(`📁  Saved at: ${finalFile}`);
}

main().catch(err => {
  console.error('\n❌ Fatal error:', err.message || err);
  process.exit(1);
});
