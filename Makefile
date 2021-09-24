.PHONY: all clean debug release test distclean dist

all: release

clean:
	@./Make.sh clean

debug: clean
	@./Make.sh debug

release: clean
	@./Make.sh release

test:
	@./Make.sh test

distclean:
	@./Make.sh distclean

dist: distclean
	@./Make.sh dist
