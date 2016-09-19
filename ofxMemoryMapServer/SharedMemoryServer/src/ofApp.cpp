#include "ofApp.h"

//string memoryKey = "ofxSharedMemory";
int memorySize = sizeof(PCLData);
bool isServer = true;
bool isConnected = false;

//--------------------------------------------------------------
void ofApp::setup(){
	ofSetWindowShape(300, 300);
	ofSetFrameRate(60);
	ofEnableAlphaBlending();
	ofSetLogLevel(OF_LOG_VERBOSE);

	// setup an object with different data types
	
	// setup + connect
	memoryMappedFile.setup(memoryKey, memorySize, isServer);
	
	//isConnected = memoryMappedFile.connect();

	ofLog() << "Memory was mapped? " << isConnected;
	ofLog() << "Memory key: " << memoryKey;
	ofLog() << "Memory size: " << memorySize;

	memClient.setup("ofxSharedMemory", memorySize, true);
	
}

//--------------------------------------------------------------
void ofApp::update(){
	ofSetWindowTitle(ofToString(isServer ? "Server connected: " : "Client connected: ") + ofToString(isConnected ? "YES" : "NO") + ", FPS: " + ofToString(ofGetFrameRate()));

	// if not connected, try reconnect every 5 seconds or so
	if (!isConnected && ofGetFrameNum() % 300 == 0) {
		isConnected = memoryMappedFile.connect();
		if (isConnected)
		{
			
			//memoryMappedFile.setData(new CustomData());
			myCustomData = memoryMappedFile.getData();
		}
		
	}

	if (!isConnected) return;

	// server updates data and saves to memory mapped file
	// client loads memory mapped file into object
	if (isServer) {

		for (int i = 0; i < NUM_POINTS; i++)
		{
			float factor = ofGetMouseX()*.03f / ofGetWidth();
			myCustomData->points[i].pos.x = sin(i*factor +ofGetElapsedTimef());
			myCustomData->points[i].pos.y = cos(i*factor*1.1 +ofGetElapsedTimef()*.5);
			myCustomData->points[i].pos.z = ofSignedNoise(i*factor*2.1 +ofGetElapsedTimef()*.25);
			myCustomData->points[i].color.r = myCustomData->points[i].pos.x + .5f;
			myCustomData->points[i].color.g = myCustomData->points[i].pos.y + .5f;
			myCustomData->points[i].color.b = myCustomData->points[i].pos.z + .5f;
			myCustomData->points[i].color.a = 1;

		}

	} else {

		myCustomData = memoryMappedFile.getData();
	}

	//ofLog() << "Client mouseX " << memClient.getData()->mouseX;
}

//--------------------------------------------------------------
void ofApp::draw(){
	// server is blue, client is yellow
	(isServer) ? ofBackground(0, 255, 255) : ofBackground(255, 255, 0);


	if (!isConnected) return;

	// ofVec3f- pink cube outline
	ofNoFill();
	ofSetColor(255, 0, 255);
	//ofBox(myCustomData->vec.x + (ofGetWidth()*.5), myCustomData->vec.y + (ofGetHeight()*.5), myCustomData->vec.z, 30);

	// mousex + mouseY- white circle
	ofFill();
	ofSetColor(255);
	//ofCircle(myCustomData->mouseX, myCustomData->mouseY, 20);

	/*
	ofSetColor(255);
	stringstream output;
	output << "Shared data..." << endl
		<< "Time : " << myCustomData->time << endl
		<< "Mouse X : " << myCustomData->mouseX << endl
		<< "Mouse Y : " << myCustomData->mouseY << endl

		//<< "Vec : " << myCustomData->vec << endl
	//	<< "Message : " << myCustomData->message[0]
		;
	ofDrawBitmapStringHighlight(output.str(), 20, 20);
	*/
}

//--------------------------------------------------------------
void ofApp::keyPressed(int key){

}

//--------------------------------------------------------------
void ofApp::keyReleased(int key){

}

//--------------------------------------------------------------
void ofApp::mouseMoved(int x, int y ){

}

//--------------------------------------------------------------
void ofApp::mouseDragged(int x, int y, int button){

}

//--------------------------------------------------------------
void ofApp::mousePressed(int x, int y, int button){

}

//--------------------------------------------------------------
void ofApp::mouseReleased(int x, int y, int button){

}

//--------------------------------------------------------------
void ofApp::mouseEntered(int x, int y){

}

//--------------------------------------------------------------
void ofApp::mouseExited(int x, int y){

}

//--------------------------------------------------------------
void ofApp::windowResized(int w, int h){

}

//--------------------------------------------------------------
void ofApp::gotMessage(ofMessage msg){

}

//--------------------------------------------------------------
void ofApp::dragEvent(ofDragInfo dragInfo){ 

}
