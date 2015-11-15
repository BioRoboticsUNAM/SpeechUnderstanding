import time
import egprs_interpreter

def interpret(s):
	return egprs_interpreter.interpret_command(s)
  

def main():
	print "Ready!"
	while True:
		# nlStr = raw_input("Enter string: ")
		nlStr = raw_input()
		cfr = interpret(nlStr)
		print cfr

if __name__ == "__main__":
  main()