var http = require('http');
var url = require("url");
var route=require("./router");

function start(route,handle){
    function onRequest(req,res){
        var pathname = url.parse(req.url).pathname;
        console.log("Request for " + pathname + " received.");
        
        route(handle,pathname,res)
        
        res.writeHead(200, {'Content-Type': 'text/plain'});
        res.end('Hello World\n');
    }
    http.createServer(onRequest).listen(process.env.PORT);
    console.log("Server has started.");
}
exports.start=start