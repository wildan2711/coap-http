var coap        = require('coap')
var http        = require('http')
var request     = require('request')
var coapServer  = coap.createServer()

var httpServerIP = "192.168.100.26"
var httpServerPort = "8080"

var suhu = 0
var lembab = 0

coapServer.on('request', function(req, res) {
    console.log(req)
    var route = req.url.split('/')[1]
    var payload = JSON.parse(req.payload) 
    if (route == "monitor") {
        suhu = payload.suhu
        lembab = payload.lembab

        console.log("suhu:",suhu)
        console.log("lembab:",lembab)
        var url = httpServerIP + ':' + httpServerPort
                    +'/monitor?suhu='
                    +suhu
                    +"&lembab="
                    +lembab
        request.get(url)
    }
})

// the default CoAP port is 5683
coapServer.listen(function() {
    console.log("Server listening on port 5683")
})