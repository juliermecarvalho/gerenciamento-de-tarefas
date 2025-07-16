# Etapa 1: build com Node + Vite
FROM node:20 AS build

WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build

# Etapa 2: servir com Nginx
FROM nginx:alpine

# Apaga conteúdo default do Nginx
RUN rm -rf /usr/share/nginx/html/*

# Copia arquivos do build para o Nginx
COPY --from=build /app/dist /usr/share/nginx/html

# Expor porta padrão HTTP
EXPOSE 80

# Inicia Nginx em modo foreground
CMD ["nginx", "-g", "daemon off;"]
