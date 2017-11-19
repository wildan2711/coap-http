var coap        = require('coap')
var http        = require('http')
var request     = require('request')
var bl          = require('bl')
var coapServerIP = "192.168.43.237:5683"
var sensorEndpoint = "/monitor"

var port = "8080"

http.createServer(function(req, res) {
    var coapReq = coap.request('coap://'+coapServerIP+sensorEndpoint)

    coapReq.on('response', function(coapRes) {
        coapRes.pipe(bl(function(err, data) {
            res.writeHead(200, {'Content-type': 'application/json'})
            res.write(data)
            res.end()
        }))
    })

    coapReq.end()
}).listen(port)

console.log("Http Server listening on port "+port)