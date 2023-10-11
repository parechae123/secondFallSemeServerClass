require('dotenv').config();     //ENV 환경설정 라이브러리 임포트

const dbHost = process.env.DB_HOST;
const dbport = process.env.DB_PORT;
const dbUser = process.env.DB_USER;
const dbPassword = process.env.DB_PASSWORD;
const dbName = process.env.DB_NAME;

console.log(dbHost);
console.log(dbport);
console.log(dbUser);
console.log(dbPassword);
console.log(dbName);