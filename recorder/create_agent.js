const fs = require('fs');
let code = fs.readFileSync('recorder/record.js', 'utf8');

code = code.replace(
  "const SITE_URL = 'https://gilsonsouzadev.github.io/portifolio/';",
  "const SITE_URL = 'http://localhost:4200/';"
);

const interception = `
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
`;

code = code.replace('const page = await browser.newPage();', 'const page = await browser.newPage();\n' + interception);

const oldAdminStart = code.indexOf("if (action === 'admin') {");
const oldAdminEnd = code.indexOf("if (action === 'edit') {");

const newAdmin = `if (action === 'admin') {
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
  `;

code = code.substring(0, oldAdminStart) + newAdmin + code.substring(oldAdminEnd);

code = code.replace("url: SITE_URL + '#/admin/login'", "url: SITE_URL + 'auth'");

// Edit mode interaction: Click on a field and edit it
const newEdit = `if (action === 'edit') {
    await sleep(1500);
    // Find an edit button (adjust selector as needed)
    try {
      const editBtn = await page.$('.btn-edit, .edit-mode-toggle, button:contains("Edit")');
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
      const saveBtn = await page.$('.btn-save, button:contains("Salvar")');
      if (saveBtn) {
        await saveBtn.click();
      }
    } catch(e) {}
    await sleep(700);
    await scrollTo(page, 400);
    await sleep(600);
  }
  `;
const oldEditStart = code.indexOf("if (action === 'edit') {");
const oldEditEnd = code.indexOf("if (action === 'end') {");
code = code.substring(0, oldEditStart) + newEdit + code.substring(oldEditEnd);

fs.writeFileSync('recorder/agent_video.js', code);
console.log('agent_video.js created successfully!');
