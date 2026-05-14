from flask import Flask, render_template, request, make_response
from redis import Redis
import os, random

app = Flask(__name__, template_folder='templates')
# Connects to the Redis container using its Kubernetes Service discovery DNS name
redis = Redis(host="redis", db=0, socket_timeout=5)

@app.route("/", methods=['POST','GET'])
def hello():
    voter_id = request.cookies.get('voter_id') or hex(random.getrandbits(64))[2:]
    vote = None

    if request.method == 'POST':
        vote = request.form['vote']
        redis.rpush('votes', f'{{"voter_id": "{voter_id}", "vote": "{vote}"}}')
    
    resp = make_response(render_template('index.html', vote=vote))
    resp.set_cookie('voter_id', voter_id)
    return resp

if __name__ == "__main__":
    app.run(host='0.0.0.0', port=80, debug=True)
