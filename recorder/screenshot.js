const puppeteer = require('puppeteer');
const path = require('path');
const fs = require('fs');

async function run() {
    const browser = await puppeteer.launch({ headless: 'new' });
    const page = await browser.newPage();
    await page.setViewport({ width: 1920, height: 1080 });
    
    const outputDir = 'C:\\Users\\gilso\\OneDrive\\Área de Trabalho\\novo produto';
    
    try {
        console.log('Navigating to Home...');
        await page.goto('https://gilsonsouzadev.github.io/Livraria-FullStack/', { waitUntil: 'networkidle0' });
        // wait for some books to load
        await new Promise(r => setTimeout(r, 2000));
        await page.screenshot({ path: path.join(outputDir, 'Print-1-Home.jpg'), type: 'jpeg', quality: 90 });
        console.log('Saved Print-1-Home.jpg');
        
        console.log('Navigating to Login...');
        await page.goto('https://gilsonsouzadev.github.io/Livraria-FullStack/login', { waitUntil: 'networkidle0' });
        await new Promise(r => setTimeout(r, 1000));
        await page.screenshot({ path: path.join(outputDir, 'Print-2-Login.jpg'), type: 'jpeg', quality: 90 });
        console.log('Saved Print-2-Login.jpg');

        console.log('Navigating to Admin...');
        await page.goto('https://gilsonsouzadev.github.io/Livraria-FullStack/admin', { waitUntil: 'networkidle0' });
        await new Promise(r => setTimeout(r, 1000));
        await page.screenshot({ path: path.join(outputDir, 'Print-3-Admin.jpg'), type: 'jpeg', quality: 90 });
        console.log('Saved Print-3-Admin.jpg');

    } catch (e) {
        console.error('Error taking screenshots:', e);
    } finally {
        await browser.close();
    }
}

run();
