var coap        = require('coap')
var http        = require('http')
var request     = require('request')
var bl          = require('bl')
var coapServerIPs = [
    "127.0.0.1:5683",
    "127.0.0.1:5684",
    "127.0.0.1:5685"
]
var sensorEndpoint = "/monitor"

var port = "8080"

http.createServer(function(req, res) {
    var id = Number(req.url.substr(1)) - 1
    var coapReq = coap.request('coap://'+coapServerIPs[id]+sensorEndpoint)

    coapReq.on('response', function(coapRes) {
        coapRes.pipe(bl(function(err, data) {
            res.writeHead(200, {'Content-type': 'application/json'})
            res.end(data)
        }))
    })

    coapReq.on('error', function(err){
        var resp = {error: 'Invalid device ID or device not connected'}
        res.writeHead(200, {'Content-type': 'application/json'})
        res.end(JSON.stringify(resp))
    })

    coapReq.end()
}).listen(port)

console.log("Http Server listening on port "+port)