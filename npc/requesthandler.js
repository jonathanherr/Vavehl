var exec = require("child_process").exec;

function start(response) {
    console.log("Request handler 'start' was called.");
    exec("ls -lah", function (error, stdout, stderr) {
        response.writeHead(200, {"Content-Type": "text/plain"});
        response.write(stdout);
        response.end();
    });
}

function move(response) {
    console.log("Request handler 'move' was called.");
    response.writeHead(200, {"Content-Type": "text/plain"});
    response.write("Hello Move");
    response.end();
}

exports.start = start;
exports.upload = move;