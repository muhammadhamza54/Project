const express = require('express');
const { Pool } = require('pg');

const app = express();

const pool = new Pool({
    connectionString: 'postgres://postgres:postgrespassword@db:5432/postgres'
});

app.use(express.static(__dirname + '/views'));

app.get('/data', async (req, res) => {
    try {
        const client = await pool.connect();

        const result = await client.query(
            'SELECT vote, COUNT(id) AS count FROM votes GROUP BY vote'
        );

        client.release();

        res.json(result.rows);
    } catch (err) {
        res.status(500).send(err.toString());
    }
});

app.listen(80, () => console.log('Listening on port 80'));