var requestHandlers = require("./requesthandler");
var router = require("./router");
var server=require("./server");
var handle = {};
var animals=require("./animals.js");
handle["/"] = requestHandlers.start;
handle["/start"] = requestHandlers.start;
handle["/move"] = requestHandlers.move;
server.start(router.route,handle);
var wolf=animals.createWolf();
var deer=animals.createDeer();
console.log(wolf.state.id);
console.log(deer.state.id);
console.log(wolf.state.id);