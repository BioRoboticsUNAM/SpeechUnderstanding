import time
import egprs_interpreter

def main():
  # print " test: ", egprs_interpreter.interpret_command("take the orange juice from the shelf and deliver it to carmen at the dining room")
  while True:
    nlStr = raw_input("Enter string: ")
    print nlStr
    print "Ok"
    cfr = egprs_interpreter.interpret_command(nlStr)
    print cfr

if __name__ == "__main__":
  main()