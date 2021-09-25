.PHONY: all clean debug release publish certificate test distclean dist

all: release

clean:
	@./Make.sh clean

debug: clean
	@./Make.sh debug

release: clean
	@./Make.sh release

publish: clean
	@./Make.sh publish

certificate:
	@./Make.sh certificate

test:
	@./Make.sh test

distclean:
	@./Make.sh distclean

dist: distclean
	@./Make.sh dist
