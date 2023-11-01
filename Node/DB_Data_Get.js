require('dotenv').config();

const mysql = require('mysql');
const express = require('express'); //express 모듈을 가져온다.
const app = express();              //app 키워드로

app.use(express.json());            //json 사용


//MYSQL 연결 설정

const pool = mysql.createPool({
    connectionLimit : 10,                   //연결 풀의 최대 커넥션 수
    host: process.env.DB_HOST,
    port: process.env.DB_PORT,
    user: process.env.DB_USER,
    password: process.env.DB_PASSWORD,
    database: process.env.DB_NAME,
});

app.get('/bag',(req,res)=>{

    console.log('req,query.user_id : '+req.query.user_id);
    pool.getConnection((err,connection)=>{
        if(err)
        {
            res.status(500).json({error : '데이터 베이스 연결 오류'});
            return;
        }
        connection.query('SELECT * FROM bags WHERE user_id = ?',[req.query.user_id],(queryErr,rows)=>{
            if(queryErr)
            {
                res.status(500).json({ error : '쿼리 오류'});
                return;
            }
            res.json({bag: rows});
        });


    });
});

app.post('/bag/add',(req,res)=>{
    console.log(req.body);

    const {user_id, item_name} = req.body;

    pool.getConnection((err,connection)=>{
        if(err)
        {
            res.status(500).json({error : '데이터 베이스 연결 오류'});
            return;
        }
        //MySQL 쿼리 실행
        connection.query('INSERT INTO `GameDB`.`bags` (`user_id`, `item_name`) VALUES (?, ?);',
        [user_id, item_name],(queryErr,results)=>{
            connection.release();
            if(queryErr)
            {
                res.status(500).json({ error : '쿼리 오류'});
                return;
            }
            res.json({message : '아이템 추가 성공'});
        });
    });
});
app.post('/bag/remove',(req,res)=>{
    console.log(req.body);
    console.log("아이템 삭제 시작");
    const {user_id, item_name} = req.body;

    pool.getConnection((err,connection)=>{
        if(err)
        {
            res.status(500).json({error : '데이터 베이스 연결 오류'});
            return;
        }
        //MySQL 쿼리 실행
        connection.query('DELETE FROM `GameDB`.`bags` WHERE  user_id= ? AND item_name = ?',
        [user_id, item_name],(queryErr,results)=>{
            connection.release();
            if(queryErr)
            {
                res.status(500).json({ error : '쿼리 오류'});
                return;
            }
            res.json({message : '아이템 삭제 성공'});
        });
    });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT,()=>{
    console.log('server is running at '+PORT+' port');
});