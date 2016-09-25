import message from "Message";
import world from "World";

message.show("Hello World");

var entity = world.getEntity("SignTrigger1");
var component = entity.getComponent("ScriptTrigger");

message.show(component.getData("Script"));

component.setData("Script", "sign2.js");
