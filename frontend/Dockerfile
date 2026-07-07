# =============================================================
# Stage 1: Build - Compila o Angular em modo produção
# =============================================================
FROM node:22-alpine AS build
WORKDIR /app

COPY package*.json ./
RUN npm ci --silent

COPY . .
RUN npm run build

# =============================================================
# Stage 2: Serve - Nginx ultra-leve para servir os arquivos estáticos
# =============================================================
FROM nginx:stable-alpine AS final

# Remove config padrão e adiciona o nosso personalizado
RUN rm /etc/nginx/conf.d/default.conf
COPY nginx.conf /etc/nginx/conf.d/nginx.conf

# Copia os arquivos buildados do Angular para o diretório do Nginx
# O Angular 20 com @angular/build gera na pasta dist/browser
COPY --from=build /app/dist/frontend/browser /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
