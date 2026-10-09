const puppeteer = require('puppeteer');
const path = require('path');
const fs = require('fs');

(async () => {
    const browser = await puppeteer.launch();
    const page = await browser.newPage();
    const htmlPath = path.resolve(__dirname, 'cv2.html');
    const pdfPath = path.resolve(__dirname, '../frontend/public/assets/docs/Gilson_Souza_Curriculo.pdf');
    const pdfPath2 = path.resolve(__dirname, '../frontend/dist/frontend/browser/assets/docs/Gilson_Souza_Curriculo.pdf');
    
    const htmlContent = fs.readFileSync(htmlPath, 'utf8');
    await page.setContent(htmlContent, { waitUntil: 'networkidle0' });
    
    const pdfBuffer = await page.pdf({
        format: 'A4',
        printBackground: true,
        margin: {
            top: '40px',
            bottom: '40px',
            left: '40px',
            right: '40px'
        }
    });
    
    fs.writeFileSync(pdfPath, pdfBuffer);
    
    if (fs.existsSync(path.dirname(pdfPath2))) {
        fs.writeFileSync(pdfPath2, pdfBuffer);
    }

    await browser.close();
    console.log('PDF generated successfully.');
})();
