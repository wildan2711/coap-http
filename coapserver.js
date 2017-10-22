var coap        = require('coap')
var http        = require('http')
var request     = require('request')
var coapServer  = coap.createServer()

var httpServerIP = "192.168.100.26"
var httpServerPort = "8080"

var data = {
    suhu: 0,
    lembaba: 0
}

coapServer.on('request', function(req, res) {
    console.log(req)
    var route = req.url.split('/')[1]
    var payload = JSON.parse(req.payload) 
    if (route == "monitor") {
        data.suhu = payload.suhu
        data.lembab = payload.lembab

    }
})

// the default CoAP port is 5683
coapServer.listen(function() {
    console.log("Coap Server listening on port 5683")
})

var http = require('http')
var port = "8080"

http.createServer(function(req, res) {
    res.end(JSON.stringify(data))
}).listen(port)

console.log("Http Server listening on port "+port)