# Copyright (c) Endjin Limited. All rights reserved.
#
# Contains data under the Norwegian licence for Open Government data (NLOD) distributed by
# the Norwegian Costal Administration - https://ais.kystverket.no/
# The license can be found at https://data.norge.no/nlod/en/2.0
# The lines in this file that contain data from this source are annotated with a comment containing "ais.kystverket.no"
# The NLOD applies only to the data in these annotated lines. The license under which you are using this software
# (either the AGPLv3, or a commercial license) applies to the whole file.

Feature: PositionReportClassAParserSpecs
    In order process AIS messages from an nm4 file
    As a developer
    I want the NmeaAisPositionReportClassAParser to be able to parse the payload section of message types 1, 2 and 3: Position Report Class A

Scenario Outline: Message Type
    When I parse '<payload>' with padding <padding> as a Position Report Class A
    Then AisPositionReportClassAParser.Type is <type>

    Examples:
    | payload                      | padding | type |
    | 1000000000000000000000000000 | 0       | 1    |
    | 1Co`hD000`0unrRcusDEcTOL0P00 | 0       | 1    |	# ais.kystverket.no

Scenario Outline: Repeat Indicator
    When I parse '<payload>' with padding <padding> as a Position Report Class A
    Then AisPositionReportClassAParser.RepeatIndicator is <repeatCount>

    Examples:
    | payload                      | padding | repeatCount |
    | 1000000000000000000000000000 | 0       | 0           |
    | 1@00000000000000000000000000 | 0       | 1           |
    | 1P00000000000000000000000000 | 0       | 2           |
    | 1h00000000000000000000000000 | 0       | 3           |
    | 13nW5<00000IoPlSbE`:P8EH0534 | 0       | 0           |	# ais.kystverket.no
    | 1Co`hD000`0unrRcusDEcTOL0P00 | 0       | 1           |	# ais.kystverket.no
    | 1Smbi200020cNi@TKh96@BEL0000 | 0       | 2           |	# ais.kystverket.no
    | 1kmbi20P@10cNiLTKgi6Wld>0000 | 0       | 3           |	# ais.kystverket.no

Scenario Outline: MMSI
    When I parse '<payload>' with padding <padding> as a Position Report Class A
    Then AisPositionReportClassAParser.Mmsi is '<mmsi>'

    Examples:
    | payload                      | padding | mmsi      |
    | 1000000000000000000000000000 | 0       | 0         |
    | 100000@000000000000000000000 | 0       | 1         |
    | 100000P000000000000000000000 | 0       | 2         |
    | 1>eq`d@000000000000000000000 | 0       | 987654321 |		# ais.kystverket.no
    | 13nW5<00000IoPlSbE`:P8EH0534 | 0       | 258590000 |		# ais.kystverket.no
    | 24c`1`001kPEGSLR98IP00462D0s | 0       | 314180000 |		# ais.kystverket.no

Scenario Outline: Navigation Status
    When I parse '<payload>' with padding <padding> as a Position Report Class A
    Then AisPositionReportClassAParser.NavigationStatus is <navigationStatus>

    Examples:
    | payload                      | padding | navigationStatus                                     |
    | 1000000000000000000000000000 | 0       | UnderwayUsingEngine                                  |
    | 1000001000000000000000000000 | 0       | AtAnchor                                             |
    | 1000002000000000000000000000 | 0       | NotUnderCommand                                      |
    | 1000003000000000000000000000 | 0       | RestrictedManoeuverability                           |
    | 1000004000000000000000000000 | 0       | ConstrainedByHerDraught                              |
    | 1000005000000000000000000000 | 0       | Moored                                               |
    | 1000006000000000000000000000 | 0       | Aground                                              |
    | 1000007000000000000000000000 | 0       | EngagedInFishing                                     |
    | 1000008000000000000000000000 | 0       | UnderWaySailing                                      |
    | 1000009000000000000000000000 | 0       | ReservedForFutureAmendmentOfNavigationalStatusForHsc |
    | 100000:000000000000000000000 | 0       | ReservedForFutureAmendmentOfNavigationalStatusForWig |
    | 100000;000000000000000000000 | 0       | ReservedForFutureUse11                               |
    | 100000<000000000000000000000 | 0       | ReservedForFutureUse12                               |
    | 100000=000000000000000000000 | 0       | ReservedForFutureUse13                               |
    | 100000>000000000000000000000 | 0       | AisSartIsActive                                      |
    | 100000?000000000000000000000 | 0       | NotDefined                                           |
    | 13nW5<00000IoPlSbE`:P8EH0534 | 0       | UnderwayUsingEngine                                  |	# ais.kystverket.no
    | 13@oSF101;PTR`fPLn2:U8S`0534 | 0       | AtAnchor                                             |	# ais.kystverket.no
    | 13mVWJ2P01P`jBlTMPsN4?v22<0G | 0       | NotUnderCommand                                      |	# ais.kystverket.no
    | 13oFwV3P?w<tSF0l4Q@>4?wv0PSu | 0       | RestrictedManoeuverability                           |	# ais.kystverket.no
    | 15CIo>401f0jO4hQ0KN<F9n:0D<P | 0       | ConstrainedByHerDraught                              |	# ais.kystverket.no
    | 13n53M50001P2jv`4iFe@rJ<0000 | 0       | Moored                                               |	# ais.kystverket.no
    | 13mTRV701F23hFp`VlU4ul><0<0; | 0       | EngagedInFishing                                     |	# ais.kystverket.no
    | 13B6BP80000jt=tPk:Isf:F@0538 | 0       | UnderWaySailing                                      |	# ais.kystverket.no
    | 13m`0o9P4hPTEKLQ>f<:ROvb0003 | 0       | ReservedForFutureAmendmentOfNavigationalStatusForHsc |	# ais.kystverket.no
    | 13mClB:Oi<Pi:U4U5n?J;8CD00Rq | 0       | ReservedForFutureAmendmentOfNavigationalStatusForWig |	# ais.kystverket.no
    | 13m6;7cP020HJ`JRs9l0A?v`08<9 | 0       | ReservedForFutureUse11                               |	# ais.kystverket.no
    | 13m9`rdw01QAv`<WiMAbF3ap2H1a | 0       | ReservedForFutureUse12                               |	# ais.kystverket.no
    | 13P=N7M1@10Ph14SqI7pAW@j0L0G | 0       | ReservedForFutureUse13                               |	# ais.kystverket.no
    | 13@njcg000PeVI>Pu9Hnul9D0D1M | 0       | NotDefined                                           |	# ais.kystverket.no

Scenario Outline: Rate of Turn
    When I parse '<payload>' with padding <padding> as a Position Report Class A
    Then AisPositionReportClassAParser.RateOfTurn is <rateOfTurn>

    Examples:
    | payload                      | padding | rateOfTurn |
    | 1000000000000000000000000000 | 0       | 0          |
    | 1000000wh0000000000000000000 | 0       | -1         |
    | 1000000Oh0000000000000000000 | 0       | 127        |
    | 1000000P00000000000000000000 | 0       | -128       |
    | 13nW5<00000IoPlSbE`:P8EH0534 | 0       | 0          |		# ais.kystverket.no
    | 13oHtV7OhN0=B9bQch;WqnCp0W3h | 0       | 127        |		# ais.kystverket.no
    | 13mCIp0P00PFnJBSHS1>4?wH2@JB | 0       | -128       |		# ais.kystverket.no
    | 13P=N7M1@10Ph14SqI7pAW@j0L0G | 0       | 5          |		# ais.kystverket.no

Scenario Outline: Speed Over Ground
    When I parse '<payload>' with padding <padding> as a Position Report Class A
    Then AisPositionReportClassAParser.SpeedOverGroundTenths is <speedOverGround>

    Examples:
    | payload                      | padding | speedOverGround |
    | 1000000000000000000000000000 | 0       | 0               |
    | 1000000001000000000000000000 | 0       | 1               |
    | 10000000?w000000000000000000 | 0       | 1023            |
    | 13nW5<00000IoPlSbE`:P8EH0534 | 0       | 0               |	# ais.kystverket.no
    | 1kmbi20P@10cNiLTKgi6Wld>0000 | 0       | 1               |	# ais.kystverket.no
    | 13@oSF101;PTR`fPLn2:U8S`0534 | 0       | 75              |	# ais.kystverket.no
    | 13m`0o9P4hPTEKLQ>f<:ROvb0003 | 0       | 304             |	# ais.kystverket.no
    | 13oFwV3P?w<tSF0l4Q@>4?wv0PSu | 0       | 1023            |	# ais.kystverket.no

Scenario Outline: Position Accuracy
    When I parse '<payload>' with padding <padding> as a Position Report Class A
    Then AisPositionReportClassAParser.PositionAccuracy is <positionAccuracy>

    Examples:
    | payload                      | padding | positionAccuracy |
    | 1000000000000000000000000000 | 0       | false            |
    | 1000000000P00000000000000000 | 0       | true             |
    | 13nW5<00000IoPlSbE`:P8EH0534 | 0       | false            |	# ais.kystverket.no
    | 13@oSF101;PTR`fPLn2:U8S`0534 | 0       | true             |	# ais.kystverket.no

Scenario Outline: Longitude and Latitute
    When I parse '<payload>' with padding <padding> as a Position Report Class A
    Then AisPositionReportClassAParser.Longitude10000thMins is <longitude>
    Then AisPositionReportClassAParser.Latitude10000thMins is <latitude>

    Examples:
    | payload                      | padding | longitude | latitude |
    | 1000000000000000000000000000 | 0       | 0         | 0        |
    | 1000000000000020000000000000 | 0       | 1         | 0        |
    | 1000000000000000000@00000000 | 0       | 0         | 1        |
    | 1000000000Owwwv0000000000000 | 0       | -1        | 0        |
    | 100000000000001wwwwh00000000 | 0       | 0         | -1       |
    | 13nW5<00000IoPlSbE`:P8EH0534 | 0       | 3390490   | 37393824 |	# ais.kystverket.no
    | 1Co`hD000`0unrRcusDEcTOL0P00 | 0       | 8107857   | 46103377 |	# ais.kystverket.no
    | 1Smbi200020cNi@TKh96@BEL0000 | 0       | 5699112   | 38203428 |	# ais.kystverket.no
    | 13oFwV3P?w<tSF0l4Q@>4?wv0PSu | 0       | 108600000 | 54600000 |	# ais.kystverket.no

Scenario Outline: Course Over Ground
    When I parse '<payload>' with padding <padding> as a Position Report Class A
    Then AisPositionReportClassAParser.CourseOverGround10thDegrees is <courseOverGround>

    Examples:
    | payload                      | padding | courseOverGround |
    | 1000000000000000000000000000 | 0       | 0                |
    | 100000000000000000000@000000 | 0       | 1                |
    | 1000000000000000000>40000000 | 0       | 3600             |
    | 1000000000000000000?wh000000 | 0       | 4095             |
    | 13mSjt7P001KOu6`6:3@0?wJ0<03 | 0       | 0                |	# ais.kystverket.no
    | 13m6;7cP020HJ`JRs9l0A?v`08<9 | 0       | 68               |	# ais.kystverket.no
    | 1Smbi200020cNi@TKh96@BEL0000 | 0       | 1601             |	# ais.kystverket.no
    | 13oO7800000FH>>S6rpur`P200S< | 0       | 3562             |	# ais.kystverket.no
    | 13mVWJ2P01P`jBlTMPsN4?v22<0G | 0       | 3600             |	# ais.kystverket.no

Scenario Outline: True Heading
    When I parse '<payload>' with padding <padding> as a Position Report Class A
    Then AisPositionReportClassAParser.TrueHeadingDegrees is <trueHeading>

    Examples:
    | payload                      | padding | trueHeading |
    | 1000000000000000000000000000 | 0       | 0           |
    | 1000000000000000000000200000 | 0       | 1           |
    | 1000000000000000000000v00000 | 0       | 31          |
    | 1000000000000000000001000000 | 0       | 32          |
    | 100000000000000000000?v00000 | 0       | 511         |
    | 1Smbi200020cNi@TKh96@BEL0000 | 0       | 74          |	# ais.kystverket.no
    | 13B6BP80000jt=tPk:Isf:F@0538 | 0       | 331         |	# ais.kystverket.no
    | 13mCIp0P00PFnJBSHS1>4?wH2@JB | 0       | 511         |	# ais.kystverket.no

Scenario Outline: Time Stamp
    When I parse '<payload>' with padding <padding> as a Position Report Class A
    Then AisPositionReportClassAParser.TimeStampSecond is <timeStamp>

    Examples:
    | payload                      | padding | timeStamp |
    | 1000000000000000000000000000 | 0       | 0         |
    | 1000000000000000000000020000 | 0       | 1         |
    | 10000000000000000000001n0000 | 0       | 59        |
    | 10000000000000000000001p0000 | 0       | 60        |
    | 10000000000000000000001v0000 | 0       | 63        |
    | 13oO7800000FH>>S6rpur`P200S< | 0       | 1         |		# ais.kystverket.no
    | 13nW5<00000IoPlSbE`:P8EH0534 | 0       | 44        |		# ais.kystverket.no
    | 13oHtV7OhN0=B9bQch;WqnCp0W3h | 0       | 60        |		# ais.kystverket.no
    | 13oFwV3P?w<tSF0l4Q@>4?wv0PSu | 0       | 63        |		# ais.kystverket.no

Scenario Outline: Manoeuvre Indicator
    When I parse '<payload>' with padding <padding> as a Position Report Class A
    Then AisPositionReportClassAParser.ManoeuvreIndicator is <manoeuvre>

    Examples:
    | payload                      | padding | manoeuvre          |
    | 1000000000000000000000000000 | 0       | NotAvailable       |
    | 100000000000000000000000P000 | 0       | NoSpecialManoeuvre |
    | 1000000000000000000000010000 | 0       | SpecialManoeuvre   |
    | 100000000000000000000001P000 | 0       | NotDefinedBySpec   |
    | 13nW5<00000IoPlSbE`:P8EH0534 | 0       | NotAvailable       |		# ais.kystverket.no
    | 13n@kJ0000PIk0@Sc2==PDQd`53L | 0       | NoSpecialManoeuvre |		# ais.kystverket.no
    | 13m69r00AfPlnG6U;43aD7MO0534 | 0       | SpecialManoeuvre   |		# ais.kystverket.no

Scenario Outline: Spare Bits at 145
    When I parse '<payload>' with padding <padding> as a Position Report Class A
    Then AisPositionReportClassAParser.SpareBits145 is <spare>

    Examples:
    | payload                      | padding | spare |
    | 1000000000000000000000000000 | 0       | 0     |
    | 1000000000000000000000004000 | 0       | 1     |
    | 1000000000000000000000008000 | 0       | 2     |
    | 100000000000000000000000<000 | 0       | 3     |
    | 100000000000000000000000@000 | 0       | 4     |
    | 100000000000000000000000D000 | 0       | 5     |
    | 100000000000000000000000H000 | 0       | 6     |
    | 100000000000000000000000L000 | 0       | 7     |
    | 13nW5<00000IoPlSbE`:P8EH0534 | 0       | 0     |		# ais.kystverket.no
    | 13n@kJ0000PIk0@Sc2==PDQd`53L | 0       | 2     |		# ais.kystverket.no

Scenario Outline: Raim Flag
    When I parse '<payload>' with padding <padding> as a Position Report Class A
    Then AisPositionReportClassAParser.RaimFlag is <flag>

    Examples:
    | payload                      | padding | flag  |
    | 1000000000000000000000000000 | 0       | false     |
    | 1000000000000000000000002000 | 0       | true      |
    | 13nW5<00000IoPlSbE`:P8EH0534 | 0       | false     |		# ais.kystverket.no
    | 13oHtV7OhN0=B9bQch;WqnCp0W3h | 0       | false     |		# ais.kystverket.no
    | 13mCIp0P00PFnJBSHS1>4?wH2@JB | 0       | true      |		# ais.kystverket.no

Scenario Outline: Radio Sync State
    When I parse '<payload>' with padding <padding> as a Position Report Class A
    Then AisPositionReportClassAParser.RadioSyncState is <state>

    Examples:
    | payload                      | padding | state          |
    | 1000000000000000000000000000 | 0       | UtcDirect      |
    | 1000000000000000000000000P00 | 0       | UtcIndirect    |
    | 1000000000000000000000001000 | 0       | ToBaseStation  |
    | 1000000000000000000000001P00 | 0       | ToOtherStation |
    | 1Smbi200020cNi@TKh96@BEL0000 | 0       | UtcDirect      |		# ais.kystverket.no
    | 1Co`hD000`0unrRcusDEcTOL0P00 | 0       | UtcIndirect    |		# ais.kystverket.no
    | 13oaO<5000PG1VhS?rC3ajf>1000 | 0       | ToBaseStation  |		# ais.kystverket.no

Scenario Outline: Radio Slot Timeout
    When I parse '<payload>' with padding <padding> as a Position Report Class A
    Then AisPositionReportClassAParser.RadioSlotTimeout is <timeout>

    Examples:
    | payload                      | padding | timeout |
    | 1000000000000000000000000000 | 0       | 0       |
    | 1000000000000000000000000400 | 0       | 1       |
    | 1000000000000000000000000800 | 0       | 2       |
    | 1000000000000000000000000<00 | 0       | 3       |
    | 1000000000000000000000000@00 | 0       | 4       |
    | 1000000000000000000000000D00 | 0       | 5       |
    | 1000000000000000000000000H00 | 0       | 6       |
    | 1000000000000000000000000L00 | 0       | 7       |
    | 13oFwV3P?w<tSF0l4Q@>4?wv0PSu | 0       | 0       |	# ais.kystverket.no
    | 13@oSF101;PTR`fPLn2:U8S`0534 | 0       | 1       |	# ais.kystverket.no
    | 13m6;7cP020HJ`JRs9l0A?v`08<9 | 0       | 2       |	# ais.kystverket.no
    | 13mTRV701F23hFp`VlU4ul><0<0; | 0       | 3       |	# ais.kystverket.no
    | 13`j@B001h0lICBPg476:TlB0@5: | 0       | 4       |	# ais.kystverket.no
    | 13@njcg000PeVI>Pu9Hnul9D0D1M | 0       | 5       |	# ais.kystverket.no
    | 13m9`rdw01QAv`<WiMAbF3ap2H1a | 0       | 6       |	# ais.kystverket.no
    | 13P=N7M1@10Ph14SqI7pAW@j0L0G | 0       | 7       |	# ais.kystverket.no

Scenario Outline: Radio Sub Message
    When I parse '<payload>' with padding <padding> as a Position Report Class A
    Then AisPositionReportClassAParser.RadioSubMessage is <message>

    Examples:
    | payload                      | padding | message |
    | 1000000000000000000000000000 | 0       | 0       |
    | 1000000000000000000000000200 | 0       | 8192    |
    | 10000000000000000000000003ww | 0       | 16383   |
    | 13mClB:Oi<Pi:U4U5n?J;8CD00Rq | 0       | 2233    |	# ais.kystverket.no