const express = require('express');
const bodyparser = require('body-parser');
const jwt = require('jsonwebtoken');
const app = express();

app.use(bodyparser.urlencoded({extended : false}));
app.use(bodyparser.json());


//사용자 데이터 (실제로는 데이터 베이스 이용 나중에) 37번째줄에서 이용하는듯
const users = [
    {username :'user1', password : 'password1'},
    {username :'user2', password : 'password2'},
];

const secretKey = "secretKey";          //비밀 키 (반드시 보안);

function verifyToken(req,res,next)      //미들웨어 함수를 사용하여 토큰 검사
{
    const token = req.headers.authorization;            //클라이언트에서 헤더에 토큰을 넣어 보낸다.

    if(!token)
    {
        return res.status(403).json({message : 'No token provided'});
    }
    jwt.verify(token,secretKey,(err,decoded)=>{
        if(err)
        {
            return res.status(401).json({message: 'failed to authenticate token'});
        }
        req.decoded == jwt.decoded;
        next();
    });
}
app.post('/login' , (req,res)=>{
    const {username,password} = req.body;
    const user = users.find(u=> u.username === username && u.password === password);        //사용자 인증 (실제로는 DB에서 검사해서 리턴)

    if(user){
        const token = jwt.sign({username: user.username}, secretKey,{expiresIn: '1h'});
        console.log(token);
        res.status(200).json({token});
    }
    else{
        res.status(401).json({message : 'login failed'})
    }
});

app.get('/protected',verifyToken,(req,res)=>{
    res.status(200).json({message: 'This is a protected endpoint', user: req.decoded});
});


const PORT = process.env.port || 3000;
app.listen(PORT,()=>{
    console.log('Server is running on port 3000');
})