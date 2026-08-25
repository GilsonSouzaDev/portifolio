const puppeteer = require('puppeteer');
const path = require('path');

async function run() {
    const browser = await puppeteer.launch({ headless: 'new' });
    const page = await browser.newPage();
    await page.setViewport({ width: 1920, height: 1080 });
    
    const outputDir = 'C:\\Users\\gilso\\OneDrive\\Área de Trabalho\\novo produto';
    
    try {
        console.log('Navigating to Catalogo...');
        await page.goto('https://gilsonsouzadev.github.io/Livraria-FullStack/catalogo', { waitUntil: 'networkidle0' });
        await new Promise(r => setTimeout(r, 2000));
        await page.screenshot({ path: path.join(outputDir, 'Print-3-Catalogo.jpg'), type: 'jpeg', quality: 90 });
        console.log('Saved Print-3-Catalogo.jpg');
        
        console.log('Navigating to Carrinho...');
        await page.goto('https://gilsonsouzadev.github.io/Livraria-FullStack/carrinho', { waitUntil: 'networkidle0' });
        await new Promise(r => setTimeout(r, 1000));
        await page.screenshot({ path: path.join(outputDir, 'Print-4-Carrinho.jpg'), type: 'jpeg', quality: 90 });
        console.log('Saved Print-4-Carrinho.jpg');

    } catch (e) {
        console.error('Error taking screenshots:', e);
    } finally {
        await browser.close();
    }
}

run();
