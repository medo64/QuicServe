.PHONY: all clean debug release publish certificate test distclean dist

all: release

clean:
	@./Make.sh clean

debug:
	@./Make.sh debug

release:
	@./Make.sh release

publish:
	@./Make.sh publish

certificate:
	@./Make.sh certificate

test:
	@./Make.sh test

distclean:
	@./Make.sh distclean

dist:
	@./Make.sh dist
