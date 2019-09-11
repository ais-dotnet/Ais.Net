# Copyright (c) Endjin Limited. All rights reserved.
#
# Contains data under the Norwegian licence for Open Government data (NLOD) distributed by
# the Norwegian Costal Administration - https://ais.kystverket.no/
# The license can be found at https://data.norge.no/nlod/en/2.0
# The lines in this file that contain data from this source are annotated with a comment containing "ais.kystverket.no"
# The NLOD applies only to the data in these annotated lines. The license under which you are using this software
# (either the AGPLv3, or a commercial license) applies to the whole file.

Feature: ParsePayloadSpecs
    In order process AIS messages from an nm4 file
    As a developer
    I want the NmeaPayloadParser to be able report the message type

Scenario Outline: Peek message type without decoding in full
	When I peek at the payload '<payload>' with padding of <padding>
	Then the message type returned by peek should be <type>

    Examples:
    | payload                                                                 | padding | type |
    | 1000000000000000000000000000                                            | 0       | 1    |
    | 13nW5<00000IoPlSbE`:P8EH0534                                            | 0       | 1    |	# ais.kystverket.no
    | 2000000000000000000000000000                                            | 0       | 2    |
    | 24c`1`001pPEGSLR:=df3@4620SQ                                            | 0       | 2    |	# ais.kystverket.no
    | 3000000000000000000000000000                                            | 0       | 3    |
    | 33m9UtPP@50wwE:VJW6LS67H01<@                                            | 0       | 3    |	# ais.kystverket.no
    | 50000000000000000000000000000000000000000000000000000000000000000000000 | 2       | 5    |
    | 53m8lk`00000hS;;SP0Hu<p61HTdTpL00000000F0H<654@pt0;@0000000000000000000 | 2       | 5    |	# ais.kystverket.no
    | B000000000000000000000000000                                            | 0       | 18   |
    | B3o8B<00F8:0h694gOtbgwqUoP06                                            | 0       | 18   |	# ais.kystverket.no
    | H000000000000000000000000000                                            | 0       | 24   |
    | H3m9b308tL5<d`4E80000000000                                             | 2       | 24   |	# ais.kystverket.no