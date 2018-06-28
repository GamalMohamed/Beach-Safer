import time
import sys
import re
from pyparrot.Mambo import Mambo
from iothub_client import IoTHubClient, IoTHubClientError, IoTHubTransportProvider, IoTHubClientResult
from iothub_client import IoTHubMessage, IoTHubMessageDispositionResult, IoTHubError, DeviceMethodReturnValue

TIMEOUT = 241000
MINIMUM_POLLING_TIME = 9
MESSAGE_TIMEOUT = 10000

RECEIVE_CONTEXT = 0
TWIN_CONTEXT = 0
SEND_REPORTED_STATE_CONTEXT = 0
METHOD_CONTEXT = 0

TWIN_CALLBACKS = 0
SEND_REPORTED_STATE_CALLBACKS = 0
METHOD_CALLBACKS = 0

PROTOCOL = IoTHubTransportProvider.MQTT
DEVICE_CONNECTION_STRING = "HostName=BSafer-iothub.azure-devices.net;DeviceId=Mambo;SharedAccessKey=LjotmEYTzL8dUYQ0j3t14DASCFSKATqo7kt9qxzfP9A="
mamboAddr = "e0:14:59:be:3d:fd"

def launchMambo():
    mambo = Mambo(mamboAddr, use_wifi=False)
    print("Connecting to mambo...")
    success = mambo.connect(num_retries=3)
    print("Connected: %s" % success)

    if (success):
        print("Initial Sleeping")
        mambo.smart_sleep(2)
        mambo.ask_for_state_update()
        mambo.smart_sleep(2)

        print("***Mission Started!*** \n")
        
        print("0. Close Claw")
        mambo.close_claw()
        mambo.smart_sleep(2)

        print("1. Taking off")
        mambo.safe_takeoff(2)
        mambo.smart_sleep(2)

        print("2. Rotating -45 degress anti-clock in-place")
        mambo.turn_degrees(-45)
        mambo.smart_sleep(2)
        
        print("3. Moving Forward 50")
        mambo.fly_direct(roll=0, pitch=100, yaw=0, vertical_movement=30, duration=1)
        mambo.smart_sleep(2)
        
        print("4. Open Claw")
        mambo.open_claw()
        mambo.smart_sleep(3)

        print("5. Close Claw")
        mambo.close_claw()
        mambo.smart_sleep(3)
        
        print("6. Rotating 180 degress anti-clock in-place")
        mambo.turn_degrees(180)
        mambo.smart_sleep(2)
        
        print("7. Moving Forward 50")
        mambo.fly_direct(roll=0, pitch=100, yaw=0, vertical_movement=-30, duration=1)
        mambo.smart_sleep(2)
        
        print("8. Rotating -135 degress anti-clock in-place")
        mambo.turn_degrees(-135)
        mambo.smart_sleep(2)
       
        print("9. Landing")
        mambo.safe_land(5)
        mambo.smart_sleep(3)

        print("***Mission Completed!***\n")

        print("Disconnecting mambo...")
        mambo.disconnect()

def device_twin_callback(update_state, payload, user_context):
    global TWIN_CALLBACKS
    print ( "\nTwin callback called with:\nupdateStatus = %s\npayload = %s\ncontext = %s" % (update_state, payload, user_context) )
    TWIN_CALLBACKS += 1
    print ( "Total calls confirmed: %d\n" % TWIN_CALLBACKS )

def send_reported_state_callback(status_code, user_context):
    global SEND_REPORTED_STATE_CALLBACKS
    print ( "Confirmation for reported state received with:\nstatus_code = [%d]\ncontext = %s" % (status_code, user_context) )
    SEND_REPORTED_STATE_CALLBACKS += 1
    print ( "    Total calls confirmed: %d" % SEND_REPORTED_STATE_CALLBACKS )

def device_method_callback(method_name, payload, user_context):
    global METHOD_CALLBACKS
    print ( "\nMethod callback called with:\nmethodName = %s\npayload = %s\ncontext = %s" % (method_name, payload, user_context) )
    METHOD_CALLBACKS += 1
    print ( "Total calls confirmed: %d\n" % METHOD_CALLBACKS )
    device_method_return_value = DeviceMethodReturnValue()
    device_method_return_value.response = "{ \"Response\": \"This is the response from the device\" }"
    device_method_return_value.status = 200
    if method_name == "takeOff":
        print ( "Invoking Direct method...\n" )
        launchMambo()
        device_method_return_value.response = "{ \"Response\": \"Successfully invoked direct method\" }"
        print("Successfully invoked direct method!")
        return device_method_return_value

    return device_method_return_value


def iothub_client_init():
    client = IoTHubClient(DEVICE_CONNECTION_STRING, PROTOCOL)
    client.set_option("product_info", "RaspberryPi")
    if client.protocol == IoTHubTransportProvider.HTTP:
        client.set_option("timeout", TIMEOUT)
        client.set_option("MinimumPollingTime", MINIMUM_POLLING_TIME)
    client.set_option("messageTimeout", MESSAGE_TIMEOUT)
    # to enable MQTT logging set to 1
    if client.protocol == IoTHubTransportProvider.MQTT:
        client.set_option("logtrace", 0)
    if client.protocol == IoTHubTransportProvider.MQTT or client.protocol == IoTHubTransportProvider.MQTT_WS:
        client.set_device_twin_callback(device_twin_callback, TWIN_CONTEXT)
        client.set_device_method_callback(device_method_callback, METHOD_CONTEXT)
    return client


def iothub_client_app_run():
    try:
        client = iothub_client_init()
        if client.protocol == IoTHubTransportProvider.MQTT:
            print ( "IoTHubClient is reporting state" )
            reported_state = "{\"newState\":\"standBy\"}"
            client.send_reported_state(reported_state, len(reported_state), send_reported_state_callback, SEND_REPORTED_STATE_CONTEXT)

        while True:
            pass

    except IoTHubError as iothub_error:
        print ( "Unexpected error %s from IoTHub" % iothub_error )
        return
    except KeyboardInterrupt:
        print ( "IoTHubClient sample stopped" )

if __name__ == "__main__":

    iothub_client_app_run()
