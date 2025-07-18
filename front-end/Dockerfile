# Etapa 1: build com Node + Vite
FROM node:20 AS build

# Diretório de trabalho
WORKDIR /app

# Copia apenas os arquivos de dependência para aproveitar cache
COPY package*.json ./

# Instala dependências
RUN npm install

# Copia restante da aplicação
COPY . .

# Executa o build (gera pasta /dist)
RUN npm run build


# Etapa 2: servir com Nginx
FROM nginx:alpine

# Remove o conteúdo padrão
RUN rm -rf /usr/share/nginx/html/*

# Copia build da aplicação para a pasta pública do Nginx
COPY --from=build /app/dist /usr/share/nginx/html

# Copia configuração personalizada do Nginx para suportar SPA
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Expõe a porta padrão
EXPOSE 80

# Inicia o Nginx em foreground
CMD ["nginx", "-g", "daemon off;"]
