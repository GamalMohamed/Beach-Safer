from pyparrot.Mambo import Mambo

mamboAddr = "e0:14:59:be:3d:fd"

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