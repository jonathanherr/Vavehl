var requestHandlers = require("./requesthandler");
var router = require("./router");
var server=require("./server");
var handle = {};
handle["/"] = requestHandlers.start;
handle["start"] = requestHandlers.start;
handle["upload"] = requestHandlers.upload;
server.start(router.route,handle);
