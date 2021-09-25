.PHONY: all clean debug release publish test distclean dist

all: release

clean:
	@./Make.sh clean

debug: clean
	@./Make.sh debug

release: clean
	@./Make.sh release

publish: clean
	@./Make.sh publish

test:
	@./Make.sh test

distclean:
	@./Make.sh distclean

dist: distclean
	@./Make.sh dist
