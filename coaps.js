var coap = require('coap')
var server = coap.createServer()

var data = {
    id: 1,
    humid: 76,
    temp: 30
}

server.on('request', function(req, res) {
    res.end(JSON.stringify(data))
})

server.listen()