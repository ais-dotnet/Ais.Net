# Copyright (c) Endjin Limited. All rights reserved.
#
# Contains data under the Norwegian licence for Open Government data (NLOD) distributed by
# the Norwegian Costal Administration - https://ais.kystverket.no/
# The license can be found at https://data.norge.no/nlod/en/2.0
# The lines in this file that contain data from this source are annotated with a comment containing "ais.kystverket.no"
# The NLOD applies only to the data in these annotated lines. The license under which you are using this software
# (either the AGPLv3, or a commercial license) applies to the whole file.

Feature: StaticDataReportParserSpecs
    In order process AIS messages from an nm4 file
    As a developer
    I want the NmeaAisStaticDataReportParser to be able to parse the payload section of message type 24: Static Data Report

# Messages come in two forms: Part A and Part B.
# Apparently the spec says all of these should be 168 bits long. However, in practice,
# it is common for transmitters to sent only 160 bits for Part A messages, leaving out
# the final 'Spare' 8 bits. In fact, in all the sample data we have I've not been able
# to find a single example of a Part A message that is 168 bits long. But allegedly
# that's what they're supposed to look like
# That's why our synthesized examples contain both 160 and 168 bit Part A messages,
# but our real examples contain only a 160 bit Part A message.

Scenario Outline: Message Part
    When I inspect the Static Data Report part of '<payload>' with padding <padding>
    Then NmeaAisStaticDataReportParser.GetPartNumber returns <partNumber>

    Examples:
    | payload                      | padding | partNumber |
    | H00000000000000000000000000  | 2       | 0          |
    | H000000000000000000000000000 | 0       | 0          |
    | H000004000000000000000000000 | 0       | 1          |
    | H000008000000000000000000000 | 0       | 2          |
    | H00000<000000000000000000000 | 0       | 3          |
    | H3mhO30PDT@V04pU@4000000000  | 2       | 0          |		# ais.kystverket.no
    | H3m<KD4NC=D5l@<<:F;000204240 | 0       | 1          |		# ais.kystverket.no

Scenario: Read Part A as Part B throws
	# ais.kystverket.no
    When I parse 'H3mhO30PDT@V04pU@4000000000' with padding 2 as Static Data Report Part B catching exception
    Then the constructor throws ArgumentException

Scenario: Read Part B as Part A throws
	# ais.kystverket.no
    When I parse 'H3m<KD4NC=D5l@<<:F;000204240' with padding 0 as Static Data Report Part A catching exception
    Then the constructor throws ArgumentException

Scenario Outline: Part A: Message Type
    When I parse '<payload>' with padding <padding> as Static Data Report Part A
    Then NmeaAisStaticDataReportParserPartA.Type is 24
    And NmeaAisStaticDataReportParserPartA.PartNumber is 0

    Examples:
    | payload                      | padding |
    | H00000000000000000000000000  | 2       |
    | H000000000000000000000000000 | 0       |
    | H3mhO30PDT@V04pU@4000000000  | 2       |		# ais.kystverket.no

Scenario Outline: Part B: Message Type
    When I parse '<payload>' with padding <padding> as Static Data Report Part B
    Then NmeaAisStaticDataReportParserPartB.Type is 24
    And NmeaAisStaticDataReportParserPartB.PartNumber is 1

    Examples:
    | payload                      | padding |
    | H000004000000000000000000000 | 0       |
    | H3m<KD4NC=D5l@<<:F;000204240 | 0       |		# ais.kystverket.no


Scenario Outline: Part A: Repeat Indicator
    When I parse '<payload>' with padding <padding> as Static Data Report Part A
    Then NmeaAisStaticDataReportParserPartA.RepeatIndicator is <repeatCount>

    Examples:
    | payload                      | padding | repeatCount |
    | H00000000000000000000000000  | 2       | 0           |
    | H000000000000000000000000000 | 0       | 0           |
    | H@0000000000000000000000000  | 2       | 1           |
    | H@00000000000000000000000000 | 0       | 1           |
    | HP0000000000000000000000000  | 2       | 2           |
    | Hh0000000000000000000000000  | 2       | 3           |
    | H3mhO30PDT@V04pU@4000000000  | 2       | 0           |	# ais.kystverket.no

Scenario Outline: Part B: Repeat Indicator
    When I parse '<payload>' with padding <padding> as Static Data Report Part B
    Then NmeaAisStaticDataReportParserPartB.RepeatIndicator is <repeatCount>

    Examples:
    | payload                      | padding | repeatCount |
    | H000004000000000000000000000 | 0       | 0           |
    | H@00004000000000000000000000 | 0       | 1           |
    | HP00004000000000000000000000 | 0       | 2           |
    | Hh00004000000000000000000000 | 0       | 3           |
    | H3m<KD4NC=D5l@<<:F;000204240 | 0       | 0           |	# ais.kystverket.no

Scenario Outline: Part A: MMSI
    When I parse '<payload>' with padding <padding> as Static Data Report Part A
    Then NmeaAisStaticDataReportParserPartA.Mmsi is <mmsi>

    Examples:
    | payload                      | padding | mmsi      |
    | H00000000000000000000000000  | 2       | 0         |
    | H00000@00000000000000000000  | 2       | 1         |
    | H00000P00000000000000000000  | 2       | 2         |
    | H>eq`d@00000000000000000000  | 2       | 987654321 |
    | H3mhO30PDT@V04pU@4000000000  | 2       | 257695500 |		# ais.kystverket.no
    | H3m8;Q1A8Tt0000000000000000  | 2       | 257035140 |		# ais.kystverket.no

Scenario Outline: Part B: MMSI
    When I parse '<payload>' with padding <padding> as Static Data Report Part B
    Then NmeaAisStaticDataReportParserPartB.Mmsi is <mmsi>

    Examples:
    | payload                      | padding | mmsi      |
    | H000004000000000000000000000 | 0       | 0         |
    | H00000D000000000000000000000 | 0       | 1         |
    | H00000T000000000000000000000 | 0       | 2         |
    | H3m<KD4NC=D5l@<<:F;000204240 | 0       | 257104720 |		# ais.kystverket.no
    | H3m76H4N@B?>1F0<;mnoh0107320 | 0       | 257017440 |		# ais.kystverket.no


Scenario Outline: Part A: Name
    When I parse '<payload>' with padding <padding> as Static Data Report Part A
    Then NmeaAisStaticDataReportParserPartA.VesselName is <vesselName>

    Examples:
    | payload                     | padding | vesselName           |
    | H00000000000000000000000000 | 2       | @@@@@@@@@@@@@@@@@@@@ |
    | H00000100000000000000000000 | 2       | P@@@@@@@@@@@@@@@@@@@ |
    | H00000300000000000000000000 | 2       | 0@@@@@@@@@@@@@@@@@@@ |
    | H00000000000000000000000010 | 2       | @@@@@@@@@@@@@@@@@@@P |
    | H00000000000000000000000004 | 2       | @@@@@@@@@@@@@@@@@@@A |
    | H3mhO30PDT@V04pU@4000000000 | 2       | HEIDI ANITA@@@@@@@@@ |	# ais.kystverket.no
    | H3m8;Q1A8Tt0000000000000000 | 2       | TRIO@@@@@@@@@@@@@@@@ |	# ais.kystverket.no

Scenario Outline: Part A: Spare
    When I parse '<payload>' with padding <padding> as Static Data Report Part A
    Then NmeaAisStaticDataReportParserPartA.Spare is <spare>

    Examples:
    | payload                      | padding | spare |
    | H00000000000000000000000000  | 2       | 0     |
    | H000000000000000000000000000 | 0       | 0     |
    | H000000000000000000000000001 | 0       | 1     |
    | H00000000000000000000000000w | 0       | 63    |
    | H000000000000000000000000010 | 0       | 64    |
    | H000000000000000000000000020 | 0       | 128   |
    | H00000000000000000000000003w | 0       | 255   |
    # No real examples available because all the Part A examples we have use the
    # non-standard but apparently ubiquitous truncated 160-bit message form.

Scenario Outline: Part B: Ship Type
    When I parse '<payload>' with padding <padding> as Static Data Report Part B
    Then NmeaAisStaticDataReportParserPartB.ShipType is <type>

    Examples:
    | payload                      | padding | type                           |
    | H000004000000000000000000000 | 0       | NotAvailable                   |
    | H000004E00000000000000000000 | 0       | WingInGroundHazardousCategoryA |
    | H3m<KD4NC=D5l@<<:F;000204240 | 0       | Fishing                        |		# ais.kystverket.no
    | H3n0Vd561=30000<7qploP1@4430 | 0       | CargoAll                       |		# ais.kystverket.no
    | H3uG2nTUCBD5l0Q00000001@4210 | 0       | PleasureCraft                  |		# ais.kystverket.no

Scenario Outline: Part B: Vendor ID ITU-R 1371-3
    When I parse '<payload>' with padding <padding> as Static Data Report Part B
    Then NmeaAisStaticDataReportParserPartB.VendorIdRev3 is <vendorId>

    Examples:
    | payload                      | padding | vendorId |
    | H000004000000000000000000000 | 0       | @@@@@@@  |
    | H0000040Q0000000000000000000 | 0       | !@@@@@@  |
    | H0000040QPPPPPP0000000000000 | 0       | !        |
    | H000004012345670000000000000 | 0       | ABCDEFG  |
    | H3m<KD4NC=D5l@<<:F;000204240 | 0       | SMTE4PL  |		# ais.kystverket.no
    | H3n0Vd561=30000<7qploP1@4430 | 0       | AMC@@@@  |		# ais.kystverket.no

Scenario Outline: Part B: Vendor ID ITU-R 1371-4
    When I parse '<payload>' with padding <padding> as Static Data Report Part B
    Then NmeaAisStaticDataReportParserPartB.VendorIdRev4 is <vendorId>

    Examples:
    | payload                      | padding | vendorId |
    | H000004000000000000000000000 | 0       | @@@      |
    | H0000040Q0000000000000000000 | 0       | !@@      |
    | H0000040QPP00000000000000000 | 0       | !        |
    | H000004012300000000000000000 | 0       | ABC      |
    | H3m<KD4NC=D5l@<<:F;000204240 | 0       | SMT      |		# ais.kystverket.no
    | H3n0Vd561=30000<7qploP1@4430 | 0       | AMC      |		# ais.kystverket.no

Scenario Outline: Part B: Unit Model Code
    When I parse '<payload>' with padding <padding> as Static Data Report Part B
    Then NmeaAisStaticDataReportParserPartB.UnitModelCode is <unitModelCode>

    Examples:
    | payload                      | padding | unitModelCode |
    | H000004000000000000000000000 | 0       | 0             |
    | H000004000040000000000000000 | 0       | 1             |
    | H000004000080000000000000000 | 0       | 2             |
    | H0000040000<0000000000000000 | 0       | 3             |
    | H0000040000P0000000000000000 | 0       | 8             |
    | H0000040000t0000000000000000 | 0       | 15            |

Scenario Outline: Part B: Serial Number
    When I parse '<payload>' with padding <padding> as Static Data Report Part B
    Then NmeaAisStaticDataReportParserPartB.SerialNumber is <serialNumber>

    Examples:
    | payload                      | padding | serialNumber |
    | H000004000000000000000000000 | 0       | 0            |
    | H000004000020000000000000000 | 0       | 524288       |
    | H000004000000010000000000000 | 0       | 1            |
    | H000004000000020000000000000 | 0       | 2            |
    | H00000400003www0000000000000 | 0       | 1048575      |

Scenario Outline: Part B: Call Sign
    When I parse '<payload>' with padding <padding> as Static Data Report Part B
    Then NmeaAisStaticDataReportParserPartB.CallSign is <callSign>

    Examples:
    | payload                      | padding | callSign |
    | H000004000000000000000000000 | 0       | @@@@@@@  |
    | H00000400000000Q000000000000 | 0       | !@@@@@@  |
    | H00000400000000QPPPPPP000000 | 0       | !        |
    | H000004000000001234567000000 | 0       | ABCDEFG  |
    | H3m<KD4NC=D5l@<<:F;000204240 | 0       | LJVK@@@  |		# ais.kystverket.no
    | H3n0Vd561=30000<7qploP1@4430 | 0       | LG9847   |		# ais.kystverket.no

Scenario Outline: Part B: Dimension to Bow
    When I parse '<payload>' with padding <padding> as Static Data Report Part B
    Then NmeaAisStaticDataReportParserPartB.DimensionToBow is <size>

    Examples:
    | payload                      | padding | size |
    | H000004000000000000000000000 | 0       | 0    |
    | H000004000000000000000080000 | 0       | 1    |
    | H000004000000000000000wp0000 | 0       | 511  |
    | H3m<KD4NC=D5l@<<:F;000204240 | 0       | 16   |		# ais.kystverket.no
    | H3n0Vd561=30000<7qploP1@4430 | 0       | 10   |		# ais.kystverket.no

Scenario Outline: Part B: Dimension to Stern
    When I parse '<payload>' with padding <padding> as Static Data Report Part B
    Then NmeaAisStaticDataReportParserPartB.DimensionToStern is <size>

    Examples:
    | payload                      | padding | size |
    | H000004000000000000000000000 | 0       | 0    |
    | H000004000000000000000001000 | 0       | 1    |
    | H00000400000000000000007w000 | 0       | 511  |
    | H3m<KD4NC=D5l@<<:F;000204240 | 0       | 4    |		# ais.kystverket.no
    | H3n0Vd561=30000<7qploP1@4430 | 0       | 4    |		# ais.kystverket.no

Scenario Outline: Part B: Dimension to Port
    When I parse '<payload>' with padding <padding> as Static Data Report Part B
    Then NmeaAisStaticDataReportParserPartB.DimensionToPort is <size>

    Examples:
    | payload                      | padding | size |
    | H000004000000000000000000000 | 0       | 0    |
    | H000004000000000000000000100 | 0       | 1    |
    | H000004000000000000000000w00 | 0       | 63   |
    | H3m<KD4NC=D5l@<<:F;000204240 | 0       | 2    |		# ais.kystverket.no
    | H3n0Vd561=30000<7qploP1@4430 | 0       | 4    |		# ais.kystverket.no

Scenario Outline: Part B: Dimension to Starboard
    When I parse '<payload>' with padding <padding> as Static Data Report Part B
    Then NmeaAisStaticDataReportParserPartB.DimensionToStarboard is <size>

    Examples:
    | payload                      | padding | size |
    | H000004000000000000000000000 | 0       | 0    |
    | H000004000000000000000000010 | 0       | 1    |
    | H0000040000000000000000000w0 | 0       | 63   |
    | H3m<KD4NC=D5l@<<:F;000204240 | 0       | 4    |		# ais.kystverket.no
    | H3n0Vd561=30000<7qploP1@4430 | 0       | 3    |		# ais.kystverket.no

Scenario Outline: Part B: Mothership MMSI
    When I parse '<payload>' with padding <padding> as Static Data Report Part B
    Then NmeaAisStaticDataReportParserPartB.MothershipMmsi is <mmsi>

    Examples:
    | payload                      | padding | mmsi      |
    | H000004000000000000000000000 | 0       | 0         |
    | H000004000000000000000000010 | 0       | 1         |
    | H000004000000000000000roVRi0 | 0       | 987654321 |

Scenario Outline: Part B: Spare
    When I parse '<payload>' with padding <padding> as Static Data Report Part B
    Then NmeaAisStaticDataReportParserPartB.Spare is <spare>

    Examples:
    | payload                      | padding | spare |
    | H000004000000000000000000000 | 0       | 0     |
    | H000004000000000000000000001 | 0       | 1     |
