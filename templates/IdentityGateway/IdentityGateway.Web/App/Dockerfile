FROM node:16

EXPOSE 4200 49153

WORKDIR /app

COPY ./package*.json ./
COPY ./.npmrc ./

RUN npm install

COPY ./ ./

CMD npm run start:docker