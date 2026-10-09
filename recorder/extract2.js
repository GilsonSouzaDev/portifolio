const fs = require('fs');
const pdfParse = require('pdf-parse');

let dataBuffer = fs.readFileSync('../frontend/public/assets/docs/Gilson_Souza_Curriculo.pdf');

pdfParse(dataBuffer).then(function(data) {
    fs.writeFileSync('cv_text.txt', data.text);
    console.log("Extraction complete.");
}).catch(console.error);
