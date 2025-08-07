# Étape 1 : build TypeScript
FROM node:18-alpine AS build

WORKDIR /app

COPY package*.json ./
RUN npm install

COPY . .
RUN npm run build

# Étape 2 : exécution avec Node
FROM node:18-alpine

WORKDIR /app

COPY --from=build /app/dist ./dist
COPY package*.json ./
RUN npm install --only=production

EXPOSE 3002

CMD ["node", "dist/server.js"]
