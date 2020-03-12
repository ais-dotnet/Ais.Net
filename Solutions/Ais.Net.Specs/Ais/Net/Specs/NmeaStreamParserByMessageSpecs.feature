# Copyright (c) Endjin Limited. All rights reserved.
#
# Contains data under the Norwegian licence for Open Government data (NLOD) distributed by
# the Norwegian Costal Administration - https://ais.kystverket.no/
# The license can be found at https://data.norge.no/nlod/en/2.0
# The lines in this file that contain data from this source are annotated with a comment containing "ais.kystverket.no"
# The NLOD applies only to the data in these annotated lines. The license under which you are using this software
# (either the AGPLv3, or a commercial license) applies to the whole file.

Feature: NmeaStreamParserByMessageSpecs
	In order to process AIS messages in NMEA files
	As a developer
	I want to be able to process each of the messages in an NMEA file

Scenario: Empty file
	Given no content
	When I parse the content by message
	Then INmeaAisMessageStreamProcessor.OnComplete should have been called
	And INmeaAisMessageStreamProcessor.OnNext should have been called 0 times

Scenario: Single CRLF blank line only
	Given a CRLF line ''
	When I parse the content by message
	Then INmeaAisMessageStreamProcessor.OnNext should have been called 0 times
	And INmeaAisMessageStreamProcessor.OnComplete should have been called

Scenario: Single LF blank line only
	Given a line ''
	When I parse the content by message
	Then INmeaAisMessageStreamProcessor.OnNext should have been called 0 times
	And INmeaAisMessageStreamProcessor.OnComplete should have been called

Scenario: Multiple CRLF blank lines only
	Given a CRLF line ''
	And a CRLF line ''
	And a CRLF line ''
	When I parse the content by message
	Then INmeaAisMessageStreamProcessor.OnNext should have been called 0 times
	And INmeaAisMessageStreamProcessor.OnComplete should have been called

Scenario: Multiple LF blank lines only
	Given a line ''
	And a line ''
	And a line ''
	When I parse the content by message
	Then INmeaAisMessageStreamProcessor.OnNext should have been called 0 times
	And INmeaAisMessageStreamProcessor.OnComplete should have been called

Scenario: Multiple mixed blank lines only
	Given a CRLF line ''
	And a line ''
	And a CRLF line ''
	When I parse the content by message
	Then INmeaAisMessageStreamProcessor.OnNext should have been called 0 times
	And INmeaAisMessageStreamProcessor.OnComplete should have been called

Scenario: Single line
	# ais.kystverket.no
	Given a line '\s:42,c:1567684904*38\!AIVDM,1,1,,A,B3m:H900AP@b:79ae6:<OwnUoP06,0*78'
	When I parse the content by message
	# ais.kystverket.no
	Then in ais message 0 the payload should be 'B3m:H900AP@b:79ae6:<OwnUoP06' with padding of 0
	Then INmeaAisMessageStreamProcessor.OnNext should have been called 1 time
	And INmeaAisMessageStreamProcessor.OnComplete should have been called

Scenario: Single line without newline only
	# ais.kystverket.no
	Given an unterminated line '\s:42,c:1567684904*38\!AIVDM,1,1,,A,B3m:H900AP@b:79ae6:<OwnUoP06,0*78'
	When I parse the content by message
	# ais.kystverket.no
	Then in ais message 0 the payload should be 'B3m:H900AP@b:79ae6:<OwnUoP06' with padding of 0
	Then INmeaAisMessageStreamProcessor.OnNext should have been called 1 time
	And INmeaAisMessageStreamProcessor.OnComplete should have been called

Scenario: Multiple lines
	# ais.kystverket.no
	Given a line '\s:42,c:1567684904*38\!AIVDM,1,1,,A,B3m:H900AP@b:79ae6:<OwnUoP06,0*78'
	# ais.kystverket.no
	And a line '\s:3,c:1567692251*01\!AIVDM,1,1,,A,13m9WS001d0K==pR=D?HB6WD0pJV,0*54'
	# ais.kystverket.no
	And a line '\s:24,c:1567692878*35\!AIVDM,1,1,,B,13o`9@701j1Ej3vc;o3q@7SJ0D02,0*21'
	# ais.kystverket.no
	And a line '\s:772,c:1567693246*07\!AIVDM,1,1,,,13o7g2001P0Lv<rSdVHf2h3N0000,0*25'
	When I parse the content by message
	Then INmeaAisMessageStreamProcessor.OnNext should have been called 4 times
	# ais.kystverket.no
	And in ais message 0 the payload should be 'B3m:H900AP@b:79ae6:<OwnUoP06' with padding of 0
	# ais.kystverket.no
	And in ais message 1 the payload should be '13m9WS001d0K==pR=D?HB6WD0pJV' with padding of 0
	# ais.kystverket.no
	And in ais message 2 the payload should be '13o`9@701j1Ej3vc;o3q@7SJ0D02' with padding of 0
	# ais.kystverket.no
	And in ais message 3 the payload should be '13o7g2001P0Lv<rSdVHf2h3N0000' with padding of 0
	And INmeaAisMessageStreamProcessor.OnComplete should have been called

Scenario: Multiple lines where final line has no newline
	# ais.kystverket.no
	Given a line '\s:42,c:1567684904*38\!AIVDM,1,1,,A,B3m:H900AP@b:79ae6:<OwnUoP06,0*78'
	# ais.kystverket.no
	And a line '\s:3,c:1567692251*01\!AIVDM,1,1,,A,13m9WS001d0K==pR=D?HB6WD0pJV,0*54'
	# ais.kystverket.no
	And a line '\s:24,c:1567692878*35\!AIVDM,1,1,,B,13o`9@701j1Ej3vc;o3q@7SJ0D02,0*21'
	# ais.kystverket.no
	And an unterminated line '\s:772,c:1567693246*07\!AIVDM,1,1,,,13o7g2001P0Lv<rSdVHf2h3N0000,0*25'
	When I parse the content by message
	Then INmeaAisMessageStreamProcessor.OnNext should have been called 4 times
	# ais.kystverket.no
	And in ais message 0 the payload should be 'B3m:H900AP@b:79ae6:<OwnUoP06' with padding of 0
	# ais.kystverket.no
	And in ais message 1 the payload should be '13m9WS001d0K==pR=D?HB6WD0pJV' with padding of 0
	# ais.kystverket.no
	And in ais message 2 the payload should be '13o`9@701j1Ej3vc;o3q@7SJ0D02' with padding of 0
	# ais.kystverket.no
	And in ais message 3 the payload should be '13o7g2001P0Lv<rSdVHf2h3N0000' with padding of 0
	And INmeaAisMessageStreamProcessor.OnComplete should have been called

Scenario: Multiple lines with blanks at start
	Given a line ''
	Given a line ''
	Given a line ''
	# ais.kystverket.no
	And a line '\s:42,c:1567684904*38\!AIVDM,1,1,,A,B3m:H900AP@b:79ae6:<OwnUoP06,0*78'
	# ais.kystverket.no
	And a line '\s:3,c:1567692251*01\!AIVDM,1,1,,A,13m9WS001d0K==pR=D?HB6WD0pJV,0*54'
	# ais.kystverket.no
	And a line '\s:24,c:1567692878*35\!AIVDM,1,1,,B,13o`9@701j1Ej3vc;o3q@7SJ0D02,0*21'
	# ais.kystverket.no
	And a line '\s:772,c:1567693246*07\!AIVDM,1,1,,,13o7g2001P0Lv<rSdVHf2h3N0000,0*25'
	When I parse the content by message
	Then INmeaAisMessageStreamProcessor.OnNext should have been called 4 times
	# ais.kystverket.no
	And in ais message 0 the payload should be 'B3m:H900AP@b:79ae6:<OwnUoP06' with padding of 0
	# ais.kystverket.no
	And in ais message 1 the payload should be '13m9WS001d0K==pR=D?HB6WD0pJV' with padding of 0
	# ais.kystverket.no
	And in ais message 2 the payload should be '13o`9@701j1Ej3vc;o3q@7SJ0D02' with padding of 0
	# ais.kystverket.no
	And in ais message 3 the payload should be '13o7g2001P0Lv<rSdVHf2h3N0000' with padding of 0
	And INmeaAisMessageStreamProcessor.OnComplete should have been called

Scenario: Multiple lines with blanks in middle
	# ais.kystverket.no
	Given a line '\s:42,c:1567684904*38\!AIVDM,1,1,,A,B3m:H900AP@b:79ae6:<OwnUoP06,0*78'
	# ais.kystverket.no
	And a line '\s:3,c:1567692251*01\!AIVDM,1,1,,A,13m9WS001d0K==pR=D?HB6WD0pJV,0*54'
	Given a line ''
	Given a line ''
	Given a line ''
	# ais.kystverket.no
	And a line '\s:24,c:1567692878*35\!AIVDM,1,1,,B,13o`9@701j1Ej3vc;o3q@7SJ0D02,0*21'
	# ais.kystverket.no
	And a line '\s:772,c:1567693246*07\!AIVDM,1,1,,,13o7g2001P0Lv<rSdVHf2h3N0000,0*25'
	When I parse the content by message
	Then INmeaAisMessageStreamProcessor.OnNext should have been called 4 times
	# ais.kystverket.no
	And in ais message 0 the payload should be 'B3m:H900AP@b:79ae6:<OwnUoP06' with padding of 0
	# ais.kystverket.no
	And in ais message 1 the payload should be '13m9WS001d0K==pR=D?HB6WD0pJV' with padding of 0
	# ais.kystverket.no
	And in ais message 2 the payload should be '13o`9@701j1Ej3vc;o3q@7SJ0D02' with padding of 0
	# ais.kystverket.no
	And in ais message 3 the payload should be '13o7g2001P0Lv<rSdVHf2h3N0000' with padding of 0
	And INmeaAisMessageStreamProcessor.OnComplete should have been called

Scenario: Multiple lines with blanks at end
	# ais.kystverket.no
	Given a line '\s:42,c:1567684904*38\!AIVDM,1,1,,A,B3m:H900AP@b:79ae6:<OwnUoP06,0*78'
	# ais.kystverket.no
	And a line '\s:3,c:1567692251*01\!AIVDM,1,1,,A,13m9WS001d0K==pR=D?HB6WD0pJV,0*54'
	# ais.kystverket.no
	And a line '\s:24,c:1567692878*35\!AIVDM,1,1,,B,13o`9@701j1Ej3vc;o3q@7SJ0D02,0*21'
	# ais.kystverket.no
	And a line '\s:772,c:1567693246*07\!AIVDM,1,1,,,13o7g2001P0Lv<rSdVHf2h3N0000,0*25'
	Given a line ''
	Given a line ''
	Given a line ''
	When I parse the content by message
	Then INmeaAisMessageStreamProcessor.OnNext should have been called 4 times
	# ais.kystverket.no
	And in ais message 0 the payload should be 'B3m:H900AP@b:79ae6:<OwnUoP06' with padding of 0
	# ais.kystverket.no
	And in ais message 1 the payload should be '13m9WS001d0K==pR=D?HB6WD0pJV' with padding of 0
	# ais.kystverket.no
	And in ais message 2 the payload should be '13o`9@701j1Ej3vc;o3q@7SJ0D02' with padding of 0
	# ais.kystverket.no
	And in ais message 3 the payload should be '13o7g2001P0Lv<rSdVHf2h3N0000' with padding of 0
	And INmeaAisMessageStreamProcessor.OnComplete should have been called

Scenario: Single unparseable line
	Given a line 'I am not an NMEA message'
	When I parse the content by message
	Then INmeaAisMessageStreamProcessor.OnNext should have been called 0 times
	Then INmeaAisMessageStreamProcessor.OnError should have been called 1 time
	And the message error report 0 should include the problematic line 'I am not an NMEA message'
	And the message error report 0 should include an exception reporting that the expected exclamation mark is missing
	And the message error report 0 should include the line number 1
	And INmeaAisMessageStreamProcessor.OnComplete should have been called

Scenario: Single truncated line
	# ais.kystverket.no
	Given a line '\s:42,c:1567684904,q:u*38\!AIVDM,1,1,,A,B'
	When I parse the content by message
	Then INmeaAisMessageStreamProcessor.OnNext should have been called 0 times
	Then INmeaAisMessageStreamProcessor.OnError should have been called 1 time
	# ais.kystverket.no
	And the message error report 0 should include the problematic line '\s:42,c:1567684904,q:u*38\!AIVDM,1,1,,A,B'
	And the message error report 0 should include an exception reporting that the message appears to be incomplete
	And the message error report 0 should include the line number 1
	And INmeaAisMessageStreamProcessor.OnComplete should have been called

Scenario: Single line where padding is missing
	# ais.kystverket.no
	Given a line '\s:42,c:1567684904*38\!AIVDM,1,1,,A,B3m:H900AP@b:79ae6:<OwnUoP06*78'
	When I parse the content by message
	Then INmeaAisMessageStreamProcessor.OnNext should have been called 0 times
	Then INmeaAisMessageStreamProcessor.OnError should have been called 1 time
	# ais.kystverket.no
	And the message error report 0 should include the problematic line '\s:42,c:1567684904*38\!AIVDM,1,1,,A,B3m:H900AP@b:79ae6:<OwnUoP06*78'
	And the message error report 0 should include an exception reporting that the padding is missing
	And the message error report 0 should include the line number 1
	And INmeaAisMessageStreamProcessor.OnComplete should have been called

Scenario: Single line where padding comma present but value missing
	# ais.kystverket.no
	Given a line '\s:42,c:1567684904*38\!AIVDM,1,1,,A,B3m:H900AP@b:79ae6:<OwnUoP06,*78'
	When I parse the content by message
	Then INmeaAisMessageStreamProcessor.OnNext should have been called 0 times
	Then INmeaAisMessageStreamProcessor.OnError should have been called 1 time
	# ais.kystverket.no
	And the message error report 0 should include the problematic line '\s:42,c:1567684904*38\!AIVDM,1,1,,A,B3m:H900AP@b:79ae6:<OwnUoP06,*78'
	And the message error report 0 should include an exception reporting that the padding is missing
	And the message error report 0 should include the line number 1
	And INmeaAisMessageStreamProcessor.OnComplete should have been called

Scenario: Single line where line truncated at padding comma
	# ais.kystverket.no
	Given a line '\s:42,c:1567684904*38\!AIVDM,1,1,,A,B3m:H900AP@b:79ae6:<OwnUoP06,'
	When I parse the content by message
	Then INmeaAisMessageStreamProcessor.OnNext should have been called 0 times
	Then INmeaAisMessageStreamProcessor.OnError should have been called 1 time
	# ais.kystverket.no
	And the message error report 0 should include the problematic line '\s:42,c:1567684904*38\!AIVDM,1,1,,A,B3m:H900AP@b:79ae6:<OwnUoP06,'
	And the message error report 0 should include an exception reporting that the padding is missing
	And the message error report 0 should include the line number 1
	And INmeaAisMessageStreamProcessor.OnComplete should have been called

Scenario: Single line where checksum is missing
	# ais.kystverket.no
	Given a line '\s:42,c:1567684904*38\!AIVDM,1,1,,A,B3m:H900AP@b:79ae6:<OwnUoP06,0'
	When I parse the content by message
	Then INmeaAisMessageStreamProcessor.OnNext should have been called 0 times
	Then INmeaAisMessageStreamProcessor.OnError should have been called 1 time
	# ais.kystverket.no
	And the message error report 0 should include the problematic line '\s:42,c:1567684904*38\!AIVDM,1,1,,A,B3m:H900AP@b:79ae6:<OwnUoP06,0'
	And the message error report 0 should include an exception reporting that the checksum is missing
	And the message error report 0 should include the line number 1
	And INmeaAisMessageStreamProcessor.OnComplete should have been called

Scenario: One unparseable line and one good line
	# ais.kystverket.no
	Given a line '\s:42,c:1567684904*38\!AIVDM,1,1,,A,B3m:H900AP@b:79ae6:<OwnUoP06,0*78'
	And a line 'I am not an NMEA message'
	When I parse the content by message
	# ais.kystverket.no
	Then in ais message 0 the payload should be 'B3m:H900AP@b:79ae6:<OwnUoP06' with padding of 0
	And INmeaAisMessageStreamProcessor.OnNext should have been called 1 time
	And INmeaAisMessageStreamProcessor.OnError should have been called 1 time
	And the message error report 0 should include the problematic line 'I am not an NMEA message'
	And the message error report 0 should include an exception reporting that the expected exclamation mark is missing
	And the message error report 0 should include the line number 2
	And INmeaAisMessageStreamProcessor.OnComplete should have been called

Scenario: Single line with non-standard tag block field with exceptions enabled
	# ais.kystverket.no
	Given a line '\s:42,c:1567684904,q:u*38\!AIVDM,1,1,,A,B3m:H900AP@b:79ae6:<OwnUoP06,0*78'
	When I parse the content by message
	# ais.kystverket.no
	Then INmeaAisMessageStreamProcessor.OnNext should have been called 0 times
	And INmeaAisMessageStreamProcessor.OnError should have been called 1 time
	And the message error report 0 should include the problematic line '\s:42,c:1567684904,q:u*38\!AIVDM,1,1,,A,B3m:H900AP@b:79ae6:<OwnUoP06,0*78'
	And the message error report 0 should include an exception reporting that an unrecognized field is present
	And the message error report 0 should include the line number 1
	And INmeaAisMessageStreamProcessor.OnComplete should have been called

Scenario: Single line with non-standard tag block field with exceptions disabled
	# ais.kystverket.no
	Given a line '\s:42,c:1567684904,q:u*38\!AIVDM,1,1,,A,B3m:H900AP@b:79ae6:<OwnUoP06,0*78'
	When I parse the content by message with exceptions disabled
	# ais.kystverket.no
	Then in ais message 0 the payload should be 'B3m:H900AP@b:79ae6:<OwnUoP06' with padding of 0
	Then INmeaAisMessageStreamProcessor.OnNext should have been called 1 time
	And INmeaAisMessageStreamProcessor.OnComplete should have been called

Scenario: Single line with standard but unsupported tag block field with exceptions enabled
	# ais.kystverket.no
	Given a line '\s:42,c:1567684904,n:1*38\!AIVDM,1,1,,A,B3m:H900AP@b:79ae6:<OwnUoP06,0*78'
	When I parse the content by message
	# ais.kystverket.no
	Then INmeaAisMessageStreamProcessor.OnNext should have been called 0 times
	And INmeaAisMessageStreamProcessor.OnError should have been called 1 time
	And the message error report 0 should include the problematic line '\s:42,c:1567684904,n:1*38\!AIVDM,1,1,,A,B3m:H900AP@b:79ae6:<OwnUoP06,0*78'
	And the message error report 0 should include an exception reporting that an unsupported field is present
	And the message error report 0 should include the line number 1
	And INmeaAisMessageStreamProcessor.OnComplete should have been called

Scenario: Single line with standard but unsupported tag block field with exceptions disabled
	# ais.kystverket.no
	Given a line '\s:42,c:1567684904,n:1*38\!AIVDM,1,1,,A,B3m:H900AP@b:79ae6:<OwnUoP06,0*78'
	When I parse the content by message with exceptions disabled
	# ais.kystverket.no
	Then in ais message 0 the payload should be 'B3m:H900AP@b:79ae6:<OwnUoP06' with padding of 0
	Then INmeaAisMessageStreamProcessor.OnNext should have been called 1 time
	And INmeaAisMessageStreamProcessor.OnComplete should have been called
