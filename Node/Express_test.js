const express = require('express'); //express 모듈을 가져온다.
const app = express();              //app 키워드로

app.use(express.json());            //json 사용

app.get('/',(req,res)=>{            //라우터 기본
    res.send('hello world!');
});
app.get('/about', (req,res)=>{          //라우터 "/about"
    res.send('about !!!');
});

app.listen(3030,()=>{
    console.log('server is running at 3030 port');
});