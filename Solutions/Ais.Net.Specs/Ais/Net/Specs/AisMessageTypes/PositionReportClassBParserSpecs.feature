# Copyright (c) Endjin Limited. All rights reserved.
#
# Contains data under the Norwegian licence for Open Government data (NLOD) distributed by
# the Norwegian Costal Administration - https://ais.kystverket.no/
# The license can be found at https://data.norge.no/nlod/en/2.0
# The lines in this file that contain data from this source are annotated with a comment containing "ais.kystverket.no"
# The NLOD applies only to the data in these annotated lines. The license under which you are using this software
# (either the AGPLv3, or a commercial license) applies to the whole file.

Feature: PositionReportClassBParserSpecs
    In order process AIS messages from an nm4 file
    As a developer
    I want the NmeaAisPositionReportClassBParser to be able to parse the payload section of message type 18: Standard Class B CS Position Report

Scenario: Message Type
    When I parse 'B3mR5u000HFQD;av`1arKwt5oP06' with padding 0 as a Position Report Class B
    Then AisPositionReportClassBParser.Type is 18

Scenario Outline: Repeat Indicator
    When I parse '<payload>' with padding <padding> as a Position Report Class B
    Then AisPositionReportClassBParser.RepeatIndicator is <repeatCount>

    Examples:
    | payload                      | padding | repeatCount |
    | B000000000000000000000000000 | 0       | 0           |
    | B@00000000000000000000000000 | 0       | 1           |
    | BP00000000000000000000000000 | 0       | 2           |
    | Bh00000000000000000000000000 | 0       | 3           |
    | B3m?;p0008<q:naAtLh03wTUoP06 | 0       | 0           |	# ais.kystverket.no
    | BCm?;p0008<q:naAtLh03wTUoP06 | 0       | 1           |	# ais.kystverket.no
    | BSm?;p0008<q:naAtLh03wTUoP06 | 0       | 2           |	# ais.kystverket.no
    | Bkm?;p0008<q:naAtLh03wTUoP06 | 0       | 3           |	# ais.kystverket.no

Scenario Outline: MMSI
    When I parse '<payload>' with padding <padding> as a Position Report Class B
    Then AisPositionReportClassBParser.Mmsi is <mmsi>

    Examples:
    | payload                      | padding | mmsi      |
    | B000000000000000000000000000 | 0       | 0         |
    | B00000@000000000000000000000 | 0       | 1         |
    | B00000P000000000000000000000 | 0       | 2         |
    | B>eq`d@000000000000000000000 | 0       | 987654321 |		# ais.kystverket.no
    | B3q35T005h<0h@`Dd:i;gwRUoP06 | 0       | 261146000 |		# ais.kystverket.no
    | B3m?;p0008<q:naAtLh03wTUoP06 | 0       | 257149920 |		# ais.kystverket.no

Scenario Outline: Regional Reserved bits 38-45
    When I parse '<payload>' with padding <padding> as a Position Report Class B
    Then AisPositionReportClassBParser.RegionalReserved38 is <reserved>

    Examples:
    | payload                      | padding | reserved |
    | B000000000000000000000000000 | 0       | 0        |
    | B000000400000000000000000000 | 0       | 1        |
    | B000001000000000000000000000 | 0       | 16       |
    | B00000?t00000000000000000000 | 0       | 255      |
    | B3m?;p0008<q:naAtLh03wTUoP06 | 0       | 0        |		# ais.kystverket.no

Scenario Outline: Speed Over Ground
    When I parse '<payload>' with padding <padding> as a Position Report Class B
    Then AisPositionReportClassBParser.SpeedOverGroundTenths is <speedOverGround>

    Examples:
    | payload                      | padding | speedOverGround |
    | B000000000000000000000000000 | 0       | 0               |
    | B00000000@000000000000000000 | 0       | 1               |
    | B0000003wh000000000000000000 | 0       | 1023            |
    | B3m?;p0008<q:naAtLh03wTUoP06 | 0       | 0               |	# ais.kystverket.no
    | B3q35T005h<0h@`Dd:i;gwRUoP06 | 0       | 23              |	# ais.kystverket.no

Scenario Outline: Position Accuracy
    When I parse '<payload>' with padding <padding> as a Position Report Class B
    Then AisPositionReportClassBParser.PositionAccuracy is <positionAccuracy>

    Examples:
    | payload                      | padding | positionAccuracy |
    | B000000000000000000000000000 | 0       | false            |
    | B000000008000000000000000000 | 0       | true             |
    | B3q35T005h<0h@`Dd:i;gwRUoP06 | 0       | false            |	# ais.kystverket.no
    | B3m?;p0008<q:naAtLh03wTUoP06 | 0       | true             |	# ais.kystverket.no

Scenario Outline: Longitude and Latitude
    When I parse '<payload>' with padding <padding> as a Position Report Class B
    Then AisPositionReportClassBParser.Longitude10000thMins is <longitude>
    Then AisPositionReportClassBParser.Latitude10000thMins is <latitude>

    Examples:
    | payload                      | padding | longitude | latitude |
    | B000000000000000000000000000 | 0       | 0         | 0        |
    | B0000000000000P0000000000000 | 0       | 1         | 0        |
    | B000000000000000004000000000 | 0       | 0         | 1        |
    | B000000007wwwwP0000000000000 | 0       | -1        | 0        |
    | B0000000000000Owwwt000000000 | 0       | 0         | -1       |
    | B3m6D@P0005r0R``WaiC;wgUkP06 | 0       | 3096645   | 36216476 |	# ais.kystverket.no

Scenario Outline: Course Over Ground
    When I parse '<payload>' with padding <padding> as a Position Report Class B
    Then AisPositionReportClassBParser.CourseOverGround10thDegrees is <courseOverGround>

    Examples:
    | payload                      | padding | courseOverGround |
    | B000000000000000000000000000 | 0       | 0                |
    | B000000000000000000040000000 | 0       | 1                |
    | B000000000000000003Pt0000000 | 0       | 3599             |
    | B000000000000000003Q00000000 | 0       | 3600             |
    | B3m?;p0008<q:naAtLh03wTUoP06 | 0       | 0                |	# ais.kystverket.no
    | B3q35T005h<0h@`Dd:i;gwRUoP06 | 0       | 1211             |	# ais.kystverket.no
    | B3m6D@P0005r0R``WaiC;wgUkP06 | 0       | 1330             |	# ais.kystverket.no
    | B3mnUc0000@LTtaWw7CQ3wVTkP06 | 0       | 3600             |	# ais.kystverket.no

Scenario Outline: True Heading
    When I parse '<payload>' with padding <padding> as a Position Report Class B
    Then AisPositionReportClassBParser.TrueHeadingDegrees is <trueHeading>

    Examples:
    | payload                      | padding | trueHeading |
    | B000000000000000000000000000 | 0       | 0           |
    | B000000000000000000000P00000 | 0       | 1           |
    | B00000000000000000003wP00000 | 0       | 511         |
    | B3m:<b0008<B<s8TO0SQ2j11nDeJ | 0       | 356         |	# ais.kystverket.no
    | B3q35T005h<0h@`Dd:i;gwRUoP06 | 0       | 511         |	# ais.kystverket.no

Scenario Outline: Time Stamp
    When I parse '<payload>' with padding <padding> as a Position Report Class B
    Then AisPositionReportClassBParser.TimeStampSecond is <timeStamp>

    Examples:
    | payload                      | padding | timeStamp |
    | B000000000000000000000000000 | 0       | 0         |
    | B0000000000000000000000P0000 | 0       | 1         |
    | B000000000000000000000MP0000 | 0       | 59        |
    | B3m:<b0008<B<s8TO0SQ2j11nDeJ | 0       | 2         |		# ais.kystverket.no
    | B3m?;p0008<q:naAtLh03wTUoP06 | 0       | 9         |		# ais.kystverket.no
    | B3mnUc0000@LTtaWw7CQ3wVTkP06 | 0       | 13        |		# ais.kystverket.no
    | B3m6D@P0005r0R``WaiC;wgUkP06 | 0       | 31        |		# ais.kystverket.no

Scenario Outline: Regional Reserved bits 139-140
    When I parse '<payload>' with padding <padding> as a Position Report Class B
    Then AisPositionReportClassBParser.RegionalReserved139 is <reserved>

    Examples:
    | payload                      | padding | reserved |
    | B000000000000000000000000000 | 0       | 0        |
    | B000000000000000000000080000 | 0       | 1        |
    | B0000000000000000000000@0000 | 0       | 2        |
    | B0000000000000000000000H0000 | 0       | 3        |
    | B3q35T005h<0h@`Dd:i;gwRUoP06 | 0       | 0        |		# ais.kystverket.no

Scenario Outline: CS unit flag
    When I parse '<payload>' with padding <padding> as a Position Report Class B
    Then AisPositionReportClassBParser.CsUnit is <unit>

    Examples:
    | payload                      | padding | unit   |
    | B000000000000000000000000000 | 0       | Sotdma |
    | B000000000000000000000040000 | 0       | Cstdma |
    | B3m:<b0008<B<s8TO0SQ2j11nDeJ | 0       | Sotdma |		# ais.kystverket.no
    | B3mnUc0000@LTtaWw7CQ3wVTkP06 | 0       | Cstdma |		# ais.kystverket.no

Scenario Outline: Display flag
    When I parse '<payload>' with padding <padding> as a Position Report Class B
    Then AisPositionReportClassBParser.HasDisplay is <hasDisplay>

    Examples:
    | payload                      | padding | hasDisplay |
    | B000000000000000000000000000 | 0       | false      |
    | B000000000000000000000020000 | 0       | true       |
    | B3q35T005h<0h@`Dd:i;gwRUoP06 | 0       | false      |		# ais.kystverket.no
    | B5RVdt0000;I;08?A>MOKws7kP06 | 0       | true       |		# ais.kystverket.no

Scenario Outline: DSC flag
    When I parse '<payload>' with padding <padding> as a Position Report Class B
    Then AisPositionReportClassBParser.IsDscAttached is <isDscAttached>

    Examples:
    | payload                      | padding | isDscAttached |
    | B000000000000000000000000000 | 0       | false         |
    | B000000000000000000000010000 | 0       | true          |
    | B3mnUc0000@LTtaWw7CQ3wVTkP06 | 0       | false         |	# ais.kystverket.no
    | B3q35T005h<0h@`Dd:i;gwRUoP06 | 0       | true          |	# ais.kystverket.no

Scenario Outline: Band flag
    When I parse '<payload>' with padding <padding> as a Position Report Class B
    Then AisPositionReportClassBParser.CanSwitchBands is <canSwitchBands>

    Examples:
    | payload                      | padding | canSwitchBands |
    | B000000000000000000000000000 | 0       | false          |
    | B00000000000000000000000P000 | 0       | true           |
    | B3q35T005h<0h@`Dd:i;gwRUoP06 | 0       | true           |		# ais.kystverket.no

Scenario Outline: Message 22 flag
    When I parse '<payload>' with padding <padding> as a Position Report Class B
    Then AisPositionReportClassBParser.CanAcceptMessage22ChannelAssignment is <canAcceptMessage22ChannelAssignment>

    Examples:
    | payload                      | padding | canAcceptMessage22ChannelAssignment |
    | B000000000000000000000000000 | 0       | false                               |
    | B00000000000000000000000@000 | 0       | true                                |
    | B3m>so00088pPR91r=JfOwV5WP06 | 0       | false                               |	# ais.kystverket.no
    | B3q35T005h<0h@`Dd:i;gwRUoP06 | 0       | true                                |	# ais.kystverket.no

Scenario Outline: Assigned flag
    When I parse '<payload>' with padding <padding> as a Position Report Class B
    Then AisPositionReportClassBParser.IsAssigned is <isAssigned>

    Examples:
    | payload                      | padding | isAssigned |
    | B000000000000000000000000000 | 0       | false      |
    | B000000000000000000000008000 | 0       | true       |
    | B3q35T005h<0h@`Dd:i;gwRUoP06 | 0       | false      |		# ais.kystverket.no

Scenario Outline: RAIM flag
    When I parse '<payload>' with padding <padding> as a Position Report Class B
    Then AisPositionReportClassBParser.RaimFlag is <raim>

    Examples:
    | payload                      | padding | raim  |
    | B000000000000000000000000000 | 0       | false |
    | B000000000000000000000004000 | 0       | true  |
    | B3m6D@P0005r0R``WaiC;wgUkP06 | 0       | false |		# ais.kystverket.no
    | B3q35T005h<0h@`Dd:i;gwRUoP06 | 0       | true  |		# ais.kystverket.no

Scenario Outline: Radio status type
    When I parse '<payload>' with padding <padding> as a Position Report Class B
    Then AisPositionReportClassBParser.RadioStatusType is <radioStatusType>

    Examples:
    | payload                      | padding | radioStatusType |
    | B000000000000000000000000000 | 0       | Sotdma          |
    | B000000000000000000000002000 | 0       | Itdma           |
    | B3q35T005h<0h@`Dd:i;gwRUoP06 | 0       | Itdma           |	# ais.kystverket.no