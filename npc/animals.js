var baseEntity=require("./entity.js");

function createWolf(){
    var wolf=new baseEntity.entity();
    wolf.init("./npc/states/wolf.json");
    return wolf;
}
function createDeer(){
    var deer=new baseEntity.entity();
    deer.init("./npc/states/deer.json");
    return deer;
}
exports.createWolf=createWolf;
exports.createDeer=createDeer;