FROM node:20-slim
WORKDIR /app
RUN npm install express pg
COPY . .
EXPOSE 80
CMD ["node", "server.js"]
