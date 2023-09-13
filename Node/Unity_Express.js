const express = require('express');
const app = express();

let users = [
    { id: 0, data: "UserData"}              //배열안에 오브젝트
];

app.use(express.json());                   //json을 사용하겠다.

app.get('/',(req ,res)=>{

    let result = {
        cmd : -1,
        message : 'Hello world'
    };

    res.send(result);
});

app.get('/id',(req ,res)=>{

    let result = {
        cmd : 100,
        message : 'MyID'
    };

    res.send(result);
});

app.post('/userdata' , (req ,res) => {
    const{id, data} = req.body;                 //Reqest Body에 Json 데이터가 들어오면 id, data에 연결
    console.log(id,data);
    let result = {
        cmd: -1,
        message: ''
    };

    let user = users.find(x=>x.id == id);       //users 배열에서 X를 찾아서 리턴

    if(user == undefined)                       //undefined 될경우 신규 등록
    {
        users.push({id , data});                //배열에 입력 Push
        result.cmd = 1001;
        result.message = "유저 신규 등록";
    }
    else
    {
        console.log(id, user.data);
        user.data = data;
        result.cmd = 1002;
        result.message = '데이터 갱신';
    }

    console.log(users);
    res.send(result);

});

app.listen(3030, ()=> {
    console.log('server is running at 3030 port');
});
