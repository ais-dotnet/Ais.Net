# Copyright (c) Endjin Limited. All rights reserved.
#
# Contains data under the Norwegian licence for Open Government data (NLOD) distributed by
# the Norwegian Costal Administration - https://ais.kystverket.no/
# The license can be found at https://data.norge.no/nlod/en/2.0
# The lines in this file that contain data from this source are annotated with a comment containing "ais.kystverket.no"
# The NLOD applies only to the data in these annotated lines. The license under which you are using this software
# (either the AGPLv3, or a commercial license) applies to the whole file.

Feature: StaticAndVoyageRelatedDataParserSpecs
    In order process AIS messages from an nm4 file
    As a developer
    I want the NmeaAisStaticAndVoyageRelatedDataParser to be able to parse the payload section of message type 5: Static and Voyage Related Data

Scenario: Message Type
	# ais.kystverket.no
    When I parse '53m89J81SR44hdhL0004eH5A84q<00000000001@4P8825LD08j0DTm0A00000000000000' with padding 2 as Static and Voyage Related Data
    Then NmeaAisStaticAndVoyageRelatedDataParser.Type is 5

Scenario Outline: Repeat Indicator
    When I parse '<payload>' with padding <padding> as Static and Voyage Related Data
    Then NmeaAisStaticAndVoyageRelatedDataParser.RepeatIndicator is <repeatCount>

    Examples:
    | payload                                                                 | padding | repeatCount |
    | 50000000000000000000000000000000000000000000000000000000000000000000000 | 2       | 0           |
    | 5@000000000000000000000000000000000000000000000000000000000000000000000 | 2       | 1           |
    | 5P000000000000000000000000000000000000000000000000000000000000000000000 | 2       | 2           |
    | 5h000000000000000000000000000000000000000000000000000000000000000000000 | 2       | 3           |
    | 53m89J81SR44hdhL0004eH5A84q<00000000001@4P8825LD08j0DTm0A00000000000000 | 2       | 0           |		# ais.kystverket.no
    | 5C@oTQT2Ad5duL9W:21=@EHq>085A@hE:222220l00P745m>0>S3kQiF@DPVAC`88888880 | 2       | 1           |		# ais.kystverket.no
    | 5SmkPJ02AmrLh=PV2208t60t@Tr222222222220l2@B785j:0BU4SkQ21BCH88888888880 | 2       | 2           |		# ais.kystverket.no

Scenario Outline: MMSI
    When I parse '<payload>' with padding <padding> as Static and Voyage Related Data
    Then NmeaAisStaticAndVoyageRelatedDataParser.Mmsi is <mmsi>

    Examples:
    | payload                                                                 | padding | mmsi      |
    | 50000000000000000000000000000000000000000000000000000000000000000000000 | 2       | 0         |
    | 500000@0000000000000000000000000000000000000000000000000000000000000000 | 2       | 1         |
    | 500000P0000000000000000000000000000000000000000000000000000000000000000 | 2       | 2         |
    | 5>eq`d@0000000000000000000000000000000000000000000000000000000000000000 | 2       | 987654321 |
    | 53m89J81SR44hdhL0004eH5A84q<00000000001@4P8825LD08j0DTm0A00000000000000 | 2       | 257034600 |		# ais.kystverket.no
    | 5C@oTQT2Ad5duL9W:21=@EHq>085A@hE:222220l00P745m>0>S3kQiF@DPVAC`88888880 | 2       | 219014278 |		# ais.kystverket.no
    | 5SmkPJ02AmrLh=PV2208t60t@Tr222222222220l2@B785j:0BU4SkQ21BCH88888888880 | 2       | 257745000 |		# ais.kystverket.no

Scenario Outline: AIS Version
    When I parse '<payload>' with padding <padding> as Static and Voyage Related Data
    Then NmeaAisStaticAndVoyageRelatedDataParser.AisVersion is <aisVersion>

    Examples:
    | payload                                                                 | padding | aisVersion |
    | 50000000000000000000000000000000000000000000000000000000000000000000000 | 2       | 0          |
    | 50000040000000000000000000000000000000000000000000000000000000000000000 | 2       | 1          |
    | 50000080000000000000000000000000000000000000000000000000000000000000000 | 2       | 2          |
    | 500000<0000000000000000000000000000000000000000000000000000000000000000 | 2       | 3          |
    | 5SmkPJ02AmrLh=PV2208t60t@Tr222222222220l2@B785j:0BU4SkQ21BCH88888888880 | 2       | 0          |		# ais.kystverket.no
    | 5C@oTQT2Ad5duL9W:21=@EHq>085A@hE:222220l00P745m>0>S3kQiF@DPVAC`88888880 | 2       | 1          |		# ais.kystverket.no
    | 53m89J81SR44hdhL0004eH5A84q<00000000001@4P8825LD08j0DTm0A00000000000000 | 2       | 2          |		# ais.kystverket.no

Scenario Outline: IMO Number
    When I parse '<payload>' with padding <padding> as Static and Voyage Related Data
    Then NmeaAisStaticAndVoyageRelatedDataParser.ImoNumber is <imoNumber>

    Examples:
    | payload                                                                 | padding | imoNumber |
    | 50000000000000000000000000000000000000000000000000000000000000000000000 | 2       | 0         |
    | 50000000000500000000000000000000000000000000000000000000000000000000000 | 2       | 1         |
    | 5000003cNJ;500000000000000000000000000000000000000000000000000000000000 | 2       | 987654321 |
    | 53m6;7`00000hEAP000488D0000000000000000l0h<335m907RCQ4QH22PDU23k3@00000 | 2       | 0         |		# ais.kystverket.no
    | 53m89J81SR44hdhL0004eH5A84q<00000000001@4P8825LD08j0DTm0A00000000000000 | 2       | 6522945   |		# ais.kystverket.no

Scenario Outline: Call Sign
    When I parse '<payload>' with padding <padding> as Static and Voyage Related Data
    Then NmeaAisStaticAndVoyageRelatedDataParser.CallSign is <callSign>

    Examples:
    | payload                                                                 | padding | callSign |
    | 50000000000000000000000000000000000000000000000000000000000000000000000 | 2       | @@@@@@@  |
    | 50000000000100000000000000000000000000000000000000000000000000000000000 | 2       | P@@@@@@  |
    | 50000000000300000000000000000000000000000000000000000000000000000000000 | 2       | 0@@@@@@  |
    | 50000000000000000040000000000000000000000000000000000000000000000000000 | 2       | @@@@@@A  |
    | 5C@oTQT2Ad5duL9W:21=@EHq>085A@hE:222220l00P745m>0>S3kQiF@DPVAC`88888880 | 2       | OWBY2    |		# ais.kystverket.no
    | 53JIbD42BlHOTP7;WSIHth622222222222222216D@L;B5n8NITSm51DQ0CH88888888880 | 2       | 9HA2986  |		# ais.kystverket.no
    | 53m89J81SR44hdhL0004eH5A84q<00000000001@4P8825LD08j0DTm0A00000000000000 | 2       | LKLG@@@  |		# ais.kystverket.no
    | 53mr4E42EBp00000001HU<Ttr0tJ1@PF0H`u8A<t00b5:40Ht3h00000000000000000000 | 2       | @@@@@@@  |		# ais.kystverket.no

Scenario Outline: Vessel Name
    When I parse '<payload>' with padding <padding> as Static and Voyage Related Data
    Then NmeaAisStaticAndVoyageRelatedDataParser.VesselName is <vesselName>

    Examples:
    | payload                                                                 | padding | vesselName           |
    | 50000000000000000000000000000000000000000000000000000000000000000000000 | 2       | @@@@@@@@@@@@@@@@@@@@ |
    | 50000000000000000010000000000000000000000000000000000000000000000000000 | 2       | P@@@@@@@@@@@@@@@@@@@ |
    | 50000000000000000030000000000000000000000000000000000000000000000000000 | 2       | 0@@@@@@@@@@@@@@@@@@@ |
    | 50000000000000000000000000000000000000400000000000000000000000000000000 | 2       | @@@@@@@@@@@@@@@@@@@A |
    | 5C@oTQT2Ad5duL9W:21=@EHq>085A@hE:222220l00P745m>0>S3kQiF@DPVAC`88888880 | 2       | STEVNS BATTLER       |	# ais.kystverket.no
    | 5SmkPJ02AmrLh=PV2208t60t@Tr222222222220l2@B785j:0BU4SkQ21BCH88888888880 | 2       | BOA ODIN             |	# ais.kystverket.no
    | 53m89J81SR44hdhL0004eH5A84q<00000000001@4P8825LD08j0DTm0A00000000000000 | 2       | AKVATRANS@@@@@@@@@@@ |	# ais.kystverket.no
    | 53mr4E42EBp00000001HU<Ttr0tJ1@PF0H`u8A<t00b5:40Ht3h00000000000000000000 | 2       | VISION OF THE FJORDS |	# ais.kystverket.no

Scenario Outline: Ship Type
    When I parse '<payload>' with padding <padding> as Static and Voyage Related Data
    Then NmeaAisStaticAndVoyageRelatedDataParser.ShipType is <type>

    Examples:
    | payload                                                                 | padding | type                           |
    | 50000000000000000000000000000000000000000000000000000000000000000000000 | 2       | NotAvailable                   |
    | 500000000000000000000000000000000000000E0000000000000000000000000000000 | 2       | WingInGroundHazardousCategoryA |
    | 53mg2o400000hOSGOJ18E=@hE=>0<P4hhDpLE:0Q0H<6640008hj<M`1Sl`2CQSp8888880 | 2       | DredgingOrUnderwaterOps        |		# ais.kystverket.no
    | 53mFoV000000hf3C3S08`u8pH`Dhh0000000000m1@6334rdR0888888888888888888880 | 2       | PortTender                     |		# ais.kystverket.no
    | 53JIbD42BlHOTP7;WSIHth622222222222222216D@L;B5n8NITSm51DQ0CH88888888880 | 2       | CargoAll                       |		# ais.kystverket.no
    | 548dvb02<<pTiT8l0008DhLT61<D5L5U<00000183jJC65mE0?DiAkm0000000000000000 | 2       | CargoHazardousCategoryB        |		# ais.kystverket.no
    | 53m7TH800000hS;3?P0Hu<p604ltp0000000001?0P;5340Ht5531Wu=N=eN=u000000000 | 2       | CargoNoAdditionalInformation   |		# ais.kystverket.no
    | 53QI:t02=oDi`@Q3B20dEV0l58Tr22222222221@8PD765lcN>4SkSRCQiC`88888888880 | 2       | TankerAll                      |		# ais.kystverket.no

Scenario Outline: Dimension to Bow
    When I parse '<payload>' with padding <padding> as Static and Voyage Related Data
    Then NmeaAisStaticAndVoyageRelatedDataParser.DimensionToBow is <size>

    Examples:
    | payload                                                                 | padding | size |
    | 50000000000000000000000000000000000000000000000000000000000000000000000 | 2       | 0    |
    | 50000000000000000000000000000000000000000800000000000000000000000000000 | 2       | 1    |
    | 5000000000000000000000000000000000000000wp00000000000000000000000000000 | 2       | 511  |
    | 5C@oTQT2Ad5duL9W:21=@EHq>085A@hE:222220l00P745m>0>S3kQiF@DPVAC`88888880 | 2       | 0    |	# ais.kystverket.no
    | 53m6;7`00000hEAP000488D0000000000000000l0h<335m907RCQ4QH22PDU23k3@00000 | 2       | 6    |	# ais.kystverket.no
    | 53JIbD42BlHOTP7;WSIHth622222222222222216D@L;B5n8NITSm51DQ0CH88888888880 | 2       | 162  |	# ais.kystverket.no

Scenario Outline: Dimension to Stern
    When I parse '<payload>' with padding <padding> as Static and Voyage Related Data
    Then NmeaAisStaticAndVoyageRelatedDataParser.DimensionToStern is <size>

    Examples:
    | payload                                                                 | padding | size |
    | 50000000000000000000000000000000000000000000000000000000000000000000000 | 2       | 0    |
    | 50000000000000000000000000000000000000000010000000000000000000000000000 | 2       | 1    |
    | 500000000000000000000000000000000000000007w0000000000000000000000000000 | 2       | 511  |
    | 53mFoV000000hf3C3S08`u8pH`Dhh0000000000m1@6334rdR0888888888888888888880 | 2       | 6    |	# ais.kystverket.no
    | 548dvb02<<pTiT8l0008DhLT61<D5L5U<00000183jJC65mE0?DiAkm0000000000000000 | 2       | 154  |	# ais.kystverket.no

Scenario Outline: Dimension to Port
    When I parse '<payload>' with padding <padding> as Static and Voyage Related Data
    Then NmeaAisStaticAndVoyageRelatedDataParser.DimensionToPort is <size>

    Examples:
    | payload                                                                 | padding | size |
    | 50000000000000000000000000000000000000000000000000000000000000000000000 | 2       | 0    |
    | 50000000000000000000000000000000000000000001000000000000000000000000000 | 2       | 1    |
    | 5000000000000000000000000000000000000000000w000000000000000000000000000 | 2       | 63   |
    | 53mFoV000000hf3C3S08`u8pH`Dhh0000000000m1@6334rdR0888888888888888888880 | 2       | 3    |	# ais.kystverket.no
    | 548dvb02<<pTiT8l0008DhLT61<D5L5U<00000183jJC65mE0?DiAkm0000000000000000 | 2       | 19   |	# ais.kystverket.no

Scenario Outline: Dimension to Starboard
    When I parse '<payload>' with padding <padding> as Static and Voyage Related Data
    Then NmeaAisStaticAndVoyageRelatedDataParser.DimensionToStarboard is <size>

    Examples:
    | payload                                                                 | padding | size |
    | 50000000000000000000000000000000000000000000000000000000000000000000000 | 2       | 0    |
    | 50000000000000000000000000000000000000000000100000000000000000000000000 | 2       | 1    |
    | 50000000000000000000000000000000000000000000w00000000000000000000000000 | 2       | 63   |
    | 53m89J81SR44hdhL0004eH5A84q<00000000001@4P8825LD08j0DTm0A00000000000000 | 2       | 2    |	# ais.kystverket.no
    | 53JIbD42BlHOTP7;WSIHth622222222222222216D@L;B5n8NITSm51DQ0CH88888888880 | 2       | 18   |	# ais.kystverket.no

Scenario Outline: Position fix type
    When I parse '<payload>' with padding <padding> as Static and Voyage Related Data
    Then NmeaAisStaticAndVoyageRelatedDataParser.PositionFixType is <epfd>

    Examples:
    | payload                                                                 | padding | epfd                       |
    | 50000000000000000000000000000000000000000000000000000000000000000000000 | 0       | Undefined                  |
    | 50000000000000000000000000000000000000000000040000000000000000000000000 | 0       | Gps                        |
    | 50000000000000000000000000000000000000000000080000000000000000000000000 | 0       | Glonass                    |
    | 500000000000000000000000000000000000000000000<0000000000000000000000000 | 0       | CombinedGpsGlonass         |
    | 500000000000000000000000000000000000000000000@0000000000000000000000000 | 0       | LoranC                     |
    | 500000000000000000000000000000000000000000000D0000000000000000000000000 | 0       | Chayka                     |
    | 500000000000000000000000000000000000000000000H0000000000000000000000000 | 0       | IntegratedNavigationSystem |
    | 500000000000000000000000000000000000000000000L0000000000000000000000000 | 0       | Surveyed                   |
    | 500000000000000000000000000000000000000000000P0000000000000000000000000 | 0       | Galileo                    |
    | 53m8:d`2F;v4hHQR220PE8l4pr0a:2222222221J0`?6600Ht8kCR81RDj1PDSDp8888880 | 0       | Undefined                  |	# ais.kystverket.no
    | 53m89J81SR44hdhL0004eH5A84q<00000000001@4P8825LD08j0DTm0A00000000000000 | 0       | Gps                        |	# ais.kystverket.no
    | 544MR0827oeaD<u0000lDdP4pTf0duAA<uH000167pF=2=nG0:0DRj0CQiC4jh000000000 | 0       | CombinedGpsGlonass         |	# ais.kystverket.no
    | 53mE09400000hoC3301<4pAV222222200000000N0h:23t0Ht6CP@000000000000000000 | 0       | 15                         |	# ais.kystverket.no

Scenario Outline: ETA Month
    When I parse '<payload>' with padding <padding> as Static and Voyage Related Data
    Then NmeaAisStaticAndVoyageRelatedDataParser.EtaMonth is <month>

    Examples:
    | payload                                                                 | padding | month |
    | 50000000000000000000000000000000000000000000000000000000000000000000000 | 2       | 0     |
    | 5000000000000000000000000000000000000000000000@000000000000000000000000 | 2       | 1     |
    | 5000000000000000000000000000000000000000000000P000000000000000000000000 | 2       | 2     |
    | 50000000000000000000000000000000000000000000010000000000000000000000000 | 2       | 4     |
    | 5000000000000000000000000000000000000000000001@000000000000000000000000 | 2       | 5     |
    | 50000000000000000000000000000000000000000000020000000000000000000000000 | 2       | 8     |
    | 50000000000000000000000000000000000000000000030000000000000000000000000 | 2       | 12    |
    | 53mr4E42EBp00000001HU<Ttr0tJ1@PF0H`u8A<t00b5:40Ht3h00000000000000000000 | 2       | 0     |		# ais.kystverket.no
    | 53m89J81SR44hdhL0004eH5A84q<00000000001@4P8825LD08j0DTm0A00000000000000 | 2       | 5     |		# ais.kystverket.no
    | 544MR0827oeaD<u0000lDdP4pTf0duAA<uH000167pF=2=nG0:0DRj0CQiC4jh000000000 | 2       | 7     |		# ais.kystverket.no

Scenario Outline: ETA Day
    When I parse '<payload>' with padding <padding> as Static and Voyage Related Data
    Then NmeaAisStaticAndVoyageRelatedDataParser.EtaDay is <day>

    Examples:
    | payload                                                                 | padding | day |
    | 50000000000000000000000000000000000000000000000000000000000000000000000 | 2       | 0   |
    | 50000000000000000000000000000000000000000000000P00000000000000000000000 | 2       | 1   |
    | 50000000000000000000000000000000000000000000001000000000000000000000000 | 2       | 2   |
    | 50000000000000000000000000000000000000000000001P00000000000000000000000 | 2       | 3   |
    | 50000000000000000000000000000000000000000000002000000000000000000000000 | 2       | 4   |
    | 50000000000000000000000000000000000000000000004000000000000000000000000 | 2       | 8   |
    | 50000000000000000000000000000000000000000000008000000000000000000000000 | 2       | 16  |
    | 5000000000000000000000000000000000000000000000?000000000000000000000000 | 2       | 30  |
    | 5000000000000000000000000000000000000000000000?P00000000000000000000000 | 2       | 31  |
    | 53mr4E42EBp00000001HU<Ttr0tJ1@PF0H`u8A<t00b5:40Ht3h00000000000000000000 | 2       | 0   |		# ais.kystverket.no
    | 53QI:t02=oDi`@Q3B20dEV0l58Tr22222222221@8PD765lcN>4SkSRCQiC`88888888880 | 2       | 9   |		# ais.kystverket.no
    | 55AQcl42D>PTQ3G7C63NmL5HE>2222222222221J28B545en08im@DQ2CQp43k0D`1CD4cP | 2       | 27  |		# ais.kystverket.no

Scenario Outline: ETA Hour
    When I parse '<payload>' with padding <padding> as Static and Voyage Related Data
    Then NmeaAisStaticAndVoyageRelatedDataParser.EtaHour is <hour>

    Examples:
    | payload                                                                 | padding | hour |
    | 50000000000000000000000000000000000000000000000000000000000000000000000 | 2       | 0    |
    | 50000000000000000000000000000000000000000000000100000000000000000000000 | 2       | 1    |
    | 50000000000000000000000000000000000000000000000200000000000000000000000 | 2       | 2    |
    | 50000000000000000000000000000000000000000000000@00000000000000000000000 | 2       | 16   |
    | 50000000000000000000000000000000000000000000000G00000000000000000000000 | 2       | 23   |
    | 50000000000000000000000000000000000000000000000H00000000000000000000000 | 2       | 24   |
    | 53mg2o400000hOSGOJ18E=@hE=>0<P4hhDpLE:0Q0H<6640008hj<M`1Sl`2CQSp8888880 | 2       | 0    |	# ais.kystverket.no
    | 53JIbD42BlHOTP7;WSIHth622222222222222216D@L;B5n8NITSm51DQ0CH88888888880 | 2       | 8    |	# ais.kystverket.no
    | 53mFoV000000hf3C3S08`u8pH`Dhh0000000000m1@6334rdR0888888888888888888880 | 2       | 12   |	# ais.kystverket.no
    | 544MR0827oeaD<u0000lDdP4pTf0duAA<uH000167pF=2=nG0:0DRj0CQiC4jh000000000 | 2       | 23   |	# ais.kystverket.no
    | 53mr4E42EBp00000001HU<Ttr0tJ1@PF0H`u8A<t00b5:40Ht3h00000000000000000000 | 2       | 24   |	# ais.kystverket.no

Scenario Outline: ETA Minute
    When I parse '<payload>' with padding <padding> as Static and Voyage Related Data
    Then NmeaAisStaticAndVoyageRelatedDataParser.EtaMinute is <minute>

    Examples:
    | payload                                                                 | padding | minute |
    | 50000000000000000000000000000000000000000000000000000000000000000000000 | 2       | 0      |
    | 50000000000000000000000000000000000000000000000010000000000000000000000 | 2       | 1      |
    | 500000000000000000000000000000000000000000000000:0000000000000000000000 | 2       | 10     |
    | 500000000000000000000000000000000000000000000000s0000000000000000000000 | 2       | 59     |
    | 500000000000000000000000000000000000000000000000t0000000000000000000000 | 2       | 60     |
    | 53m89J81SR44hdhL0004eH5A84q<00000000001@4P8825LD08j0DTm0A00000000000000 | 2       | 0      |		# ais.kystverket.no
    | 53mFoV000000hf3C3S08`u8pH`Dhh0000000000m1@6334rdR0888888888888888888880 | 2       | 34     |		# ais.kystverket.no
    | 53mr4E42EBp00000001HU<Ttr0tJ1@PF0H`u8A<t00b5:40Ht3h00000000000000000000 | 2       | 60     |		# ais.kystverket.no

Scenario Outline: Draught
    When I parse '<payload>' with padding <padding> as Static and Voyage Related Data
    Then NmeaAisStaticAndVoyageRelatedDataParser.Draught10thMetres is <draught>

    Examples:
    | payload                                                                 | padding | draught |
    | 50000000000000000000000000000000000000000000000000000000000000000000000 | 2       | 0       |
    | 50000000000000000000000000000000000000000000000000@00000000000000000000 | 2       | 1       |
    | 50000000000000000000000000000000000000000000000000P00000000000000000000 | 2       | 2       |
    | 50000000000000000000000000000000000000000000000000h00000000000000000000 | 2       | 3       |
    | 50000000000000000000000000000000000000000000000001000000000000000000000 | 2       | 4       |
    | 5000000000000000000000000000000000000000000000000P000000000000000000000 | 2       | 128     |
    | 5000000000000000000000000000000000000000000000000wh00000000000000000000 | 2       | 255     |
    | 53mFoV000000hf3C3S08`u8pH`Dhh0000000000m1@6334rdR0888888888888888888880 | 2       | 0       |		# ais.kystverket.no
    | 53mr4E42EBp00000001HU<Ttr0tJ1@PF0H`u8A<t00b5:40Ht3h00000000000000000000 | 2       | 15      |		# ais.kystverket.no
    | 53JIbD42BlHOTP7;WSIHth622222222222222216D@L;B5n8NITSm51DQ0CH88888888880 | 2       | 102     |		# ais.kystverket.no

Scenario Outline: Destination
    When I parse '<payload>' with padding <padding> as Static and Voyage Related Data
    Then NmeaAisStaticAndVoyageRelatedDataParser.Destination is <destination>

    Examples:
    | payload                                                                 | padding | destination          |
    | 50000000000000000000000000000000000000000000000000000000000000000000000 | 2       | @@@@@@@@@@@@@@@@@@@@ |
    | 50000000000000000000000000000000000000000000000000<00000000000000000000 | 2       | 0@@@@@@@@@@@@@@@@@@@ |
    | 5000000000000000000000000000000000000000000000000000000000000000000000@ | 2       | @@@@@@@@@@@@@@@@@@@A |
    | 53mr4E42EBp00000001HU<Ttr0tJ1@PF0H`u8A<t00b5:40Ht3h00000000000000000000 | 2       | @@@@@@@@@@@@@@@@@@@@ |	# ais.kystverket.no
    | 53m89J81SR44hdhL0004eH5A84q<00000000001@4P8825LD08j0DTm0A00000000000000 | 2       | HARSTAD@@@@@@@@@@@@@ |	# ais.kystverket.no
    | 5SmkPJ02AmrLh=PV2208t60t@Tr222222222220l2@B785j:0BU4SkQ21BCH88888888880 | 2       | TRONDHEIM            |	# ais.kystverket.no
    | 53mFoV000000hf3C3S08`u8pH`Dhh0000000000m1@6334rdR0888888888888888888880 | 2       |                      |	# ais.kystverket.no

Scenario Outline: DTE ready
    When I parse '<payload>' with padding <padding> as Static and Voyage Related Data
    Then NmeaAisStaticAndVoyageRelatedDataParser.DteNotReady is <notReady>

    Examples:
    | payload                                                                 | padding | notReady |
    | 50000000000000000000000000000000000000000000000000000000000000000000000 | 2       | false    |
    | 50000000000000000000000000000000000000000000000000000000000000000000008 | 2       | true     |
    | 53m89J81SR44hdhL0004eH5A84q<00000000001@4P8825LD08j0DTm0A00000000000000 | 2       | false    |	# ais.kystverket.no
    | 53m:<F82FRLThI1:220puH60l5=@E:222222221J0`D460rdR8p88888888888888888888 | 2       | true     |	# ais.kystverket.no

Scenario Outline: Spare
    When I parse '<payload>' with padding <padding> as Static and Voyage Related Data
    Then NmeaAisStaticAndVoyageRelatedDataParser.Spare423 is <spare>

    Examples:
    | payload                                                                 | padding | spare |
    | 50000000000000000000000000000000000000000000000000000000000000000000000 | 2       | 0     |
    | 50000000000000000000000000000000000000000000000000000000000000000000004 | 2       | 1     |
    | 53m89J81SR44hdhL0004eH5A84q<00000000001@4P8825LD08j0DTm0A00000000000000 | 2       | 0     |	# ais.kystverket.no
    | 5=7LHAT000000000000L58A<uT6085B3@000000U0@21140Ht000000000000000000000< | 2       | 1     |	# ais.kystverket.no