var fs=require("fs");
function entity(){
    this.states={};
    this.transitions={};
    this.state={};
    this.stimuli={};
    this.motorCenter=function(){
        //control autonomic behavior - moving, eating, etc. 
    };
    
    this.languageCenter=function(){
        //control conversations, higher logic
    };
    
    this.init=function(stateFile){
        this.state=JSON.parse(fs.readFileSync(stateFile));
        console.log("id:"+this.state.id);
        this.states=this.state.states;
        this.transitions=this.state.transitions;
        this.stimuli=this.state.stimuli;
    };
    this.do=function(state){
        console.log("doing " + state);
    };
    this.changeState=function(newState){
        console.log("changing from " + this.state.currentState + " to " + newState);
    };
    this.status=function(){
        console.log("my state is " + this.state.currentState);
    };
};

exports.entity=entity;