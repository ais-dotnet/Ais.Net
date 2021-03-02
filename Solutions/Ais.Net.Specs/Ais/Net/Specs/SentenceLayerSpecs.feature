# Copyright (c) Endjin Limited. All rights reserved.
#
# Contains data under the Norwegian licence for Open Government data (NLOD) distributed by
# the Norwegian Costal Administration - https://ais.kystverket.no/
# The license can be found at https://data.norge.no/nlod/en/2.0
# The lines in this file that contain data from this source are annotated with a comment containing "ais.kystverket.no"
# The NLOD applies only to the data in these annotated lines. The license under which you are using this software
# (either the AGPLv3, or a commercial license) applies to the whole file.

Feature: SentenceLayerSpecs
    In order process AIS messages from an nm4 file
    As a developer
    I want to the NmeaLineParser to be able to identify the main parts of a line from an NMEA file

Scenario: Tag block not present
	When I parse a message with no tag block
	Then the TagBlockWithoutDelimiters property's Length should be 0

Scenario: Tag block present
	When I parse a message with a tag block
	Then the TagBlockWithoutDelimiters property should match the tag block without the delimiters

Scenario Outline: AIS talker id
	When I parse a message with a packet tag field of '<tag>'
	Then the AisTalker is '<talkerId>'

	Examples:
		| tag   | talkerId               |
		| ABVDM | BaseStation            |
		| ADVDM | DependentBaseStation   |
		| AIVDM | MobileStation          |
		| ANVDM | AidToNavigationStation |
		| ARVDM | ReceivingStation       |
		| ASVDM | LimitedBaseStation     |
		| ATVDM | TransmittingStation    |
		| AXVDM | RepeaterStation        |
		| BSVDM | DeprecatedBaseStation  |
		| SAVDM | PhysicalShoreStation   |

Scenario Outline: AIS Origin
	When I parse a message with a packet tag field of '<tag>'
	Then the DataOrigin is '<dataOrigin>'

	Examples:
		| tag   | dataOrigin |
		| AIVDM | Vdm        |
		| AIVDO | Vdo        |

# ais.kystverket.no
# \s:42,c:1567684904*38\!AIVDM,1,1,,A,B3m:H900AP@b:79ae6:<OwnUoP06,0*78
Scenario: Non-fragmented messages
	When I parse a non-fragmented message
	Then the TotalFragmentCount is '1'
	And the FragmentNumberOneBased is '1'
	And the MultiSequenceMessageId is empty
	And the TagBlockSentenceGrouping is not present

# E.g.
# ais.kystverket.no
# \g:1-2-8055,s:99,c:1567685556*4E\!AIVDM,2,1,6,B,53oGfN42=WRDhlHn221<4i@Dr22222222222220`1@O6640Ht50Skp4mR`4l,0*72
# ais.kystverket.no
# \g:2-2-8055*55\!AIVDM,2,2,6,B,j`888888880,2*2B
# NMEA 4.10 defines the 'g' tag in the tag block as being <sentence-number>-<total-sentences-in-group>-<group-id>.
# So in the example above we have:
#   Sentence number     Total Sentences     Group id
#       1                   2                   8055
#       2                   2                   8055
#
# The AIVDM/AIVDO sentence layer has the same concept but slightly different nomenclature (and arbitrarily swaps the
# order of the first two fields, so for those same two messages we'd say:
#   Total fragments     Fragment number     Message ID
#       2                   1                   6
#       2                   2                   6
Scenario Outline: Fragmented messages
	When I parse a message fragment part <currentFragment> of <totalFragments> with message id <sequentialMessageId> and sentence group id <sentenceGroupId>
	Then the TotalFragmentCount is '<totalFragments>'
	And the FragmentNumberOneBased is '<currentFragment>'
	And the MultiSequenceMessageId is '<sequentialMessageId>'
	And the SentenceGroupId is '<sentenceGroupId>'

	Examples:
		| totalFragments | currentFragment | sequentialMessageId | sentenceGroupId |
		| 2              | 1               | 6                   | 8055            |
		| 2              | 2               | 6                   | 8055            |
		| 3              | 1               | 0                   | 3451            |
		| 3              | 2               | 0                   | 3451            |
		| 3              | 3               | 0                   | 3451            |

# Some sources produce fragmented messages without the sentence group IDs in the header, so we have to rely purely on
# the fragment IDs in the AIVDM/AIVDO layer

Scenario Outline: Fragmented messages without group ids in header
	When I parse a message fragment part <currentFragment> of <totalFragments> with message id <sequentialMessageId> and no sentence group id
	Then the TotalFragmentCount is '<totalFragments>'
	And the FragmentNumberOneBased is '<currentFragment>'
	And the MultiSequenceMessageId is '<sequentialMessageId>'

	Examples:
		| totalFragments | currentFragment | sequentialMessageId |
		| 2              | 1               | 6                   |
		| 2              | 2               | 6                   |
		| 3              | 1               | 0                   |
		| 3              | 2               | 0                   |
		| 3              | 3               | 0                   |


Scenario Outline: Radio channel code
	When I parse a message with a radio channel code of '<channelCode>'
	Then the ChannelCode is '<channelCode>'

	Examples:
		| channelCode |
		| A           |
		| B           |
		| 1           |
		| 2           |

Scenario: Payload
# ai	s.kystverket.no
	When I parse a message with a payload of 'B3m:H900AP@b:79ae6:<OwnUoP06'
	# ais.kystverket.no
	Then the payload is 'B3m:H900AP@b:79ae6:<OwnUoP06'

Scenario: Padding
	When I parse a message with padding of 3
	Then the padding is 3