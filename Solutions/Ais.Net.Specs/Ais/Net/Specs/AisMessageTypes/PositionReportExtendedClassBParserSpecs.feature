# Copyright (c) Endjin Limited. All rights reserved.

Feature: PositionReportExtendedClassBParserSpecs
    In order process AIS messages from an nm4 file
    As a developer
    I want the NmeaAisPositionReportExtendedClassBParser to be able to parse the payload section of message type 19: Extended Class B CS Position Report

Scenario: Message Type
    When I parse 'C>eq`d@000000000000000000000000000000000000000000000' with padding 0 as a Position Report Extended Class B
    Then NmeaAisPositionReportExtendedClassBParser.Type is 19

Scenario Outline: Repeat Indicator
    When I parse '<payload>' with padding <padding> as a Position Report Extended Class B
    Then NmeaAisPositionReportExtendedClassBParser.RepeatIndicator is <repeatCount>

    Examples:
    | payload                                              | padding | repeatCount |
    | C000000000000000000000000000000000000000000000000000 | 0       | 0           |
    | C@00000000000000000000000000000000000000000000000000 | 0       | 1           |
    | CP00000000000000000000000000000000000000000000000000 | 0       | 2           |
    | Ch00000000000000000000000000000000000000000000000000 | 0       | 3           |

Scenario Outline: MMSI
    When I parse '<payload>' with padding <padding> as a Position Report Extended Class B
    Then NmeaAisPositionReportExtendedClassBParser.Mmsi is <mmsi>

    Examples:
    | payload                                              | padding | mmsi      |
    | C000000000000000000000000000000000000000000000000000 | 0       | 0         |
    | C00000@000000000000000000000000000000000000000000000 | 0       | 1         |
    | C00000P000000000000000000000000000000000000000000000 | 0       | 2         |
    | C>eq`d@000000000000000000000000000000000000000000000 | 0       | 987654321 |

Scenario Outline: Regional Reserved bits 38-45
    When I parse '<payload>' with padding <padding> as a Position Report Extended Class B
    Then NmeaAisPositionReportExtendedClassBParser.RegionalReserved38 is <reserved>

    Examples:
    | payload                                              | padding | reserved |
    | C000000000000000000000000000000000000000000000000000 | 0       | 0        |
    | C000000400000000000000000000000000000000000000000000 | 0       | 1        |
    | C000001000000000000000000000000000000000000000000000 | 0       | 16       |
    | C00000?t00000000000000000000000000000000000000000000 | 0       | 255      |

Scenario Outline: Speed Over Ground
    When I parse '<payload>' with padding <padding> as a Position Report Extended Class B
    Then NmeaAisPositionReportExtendedClassBParser.SpeedOverGroundTenths is <speedOverGround>

    Examples:
    | payload                                              | padding | speedOverGround |
    | C000000000000000000000000000000000000000000000000000 | 0       | 0               |
    | C00000000@000000000000000000000000000000000000000000 | 0       | 1               |
    | C0000003wh000000000000000000000000000000000000000000 | 0       | 1023            |

Scenario Outline: Position Accuracy
    When I parse '<payload>' with padding <padding> as a Position Report Extended Class B
    Then NmeaAisPositionReportExtendedClassBParser.PositionAccuracy is <positionAccuracy>

    Examples:
    | payload                                              | padding | positionAccuracy |
    | C000000000000000000000000000000000000000000000000000 | 0       | false            |
    | C000000008000000000000000000000000000000000000000000 | 0       | true             |

Scenario Outline: Longitude and Latitute
    When I parse '<payload>' with padding <padding> as a Position Report Extended Class B
    Then NmeaAisPositionReportExtendedClassBParser.Longitude10000thMins is <longitude>
    Then NmeaAisPositionReportExtendedClassBParser.Latitude10000thMins is <latitude>

    Examples:
    | payload                                              | padding | longitude | latitude |
    | C000000000000000000000000000000000000000000000000000 | 0       | 0         | 0        |
    | C0000000000000P0000000000000000000000000000000000000 | 0       | 1         | 0        |
    | C000000000000000004000000000000000000000000000000000 | 0       | 0         | 1        |
    | C000000007wwwwP0000000000000000000000000000000000000 | 0       | -1        | 0        |
    | C0000000000000Owwwt000000000000000000000000000000000 | 0       | 0         | -1       |

Scenario Outline: Course Over Ground
    When I parse '<payload>' with padding <padding> as a Position Report Extended Class B
    Then NmeaAisPositionReportExtendedClassBParser.CourseOverGround10thDegrees is <courseOverGround>

    Examples:
    | payload                                              | padding | courseOverGround |
    | C000000000000000000000000000000000000000000000000000 | 0       | 0                |
    | C000000000000000000040000000000000000000000000000000 | 0       | 1                |
    | C000000000000000003Pt0000000000000000000000000000000 | 0       | 3599             |
    | C000000000000000003Q00000000000000000000000000000000 | 0       | 3600             |

Scenario Outline: True Heading
    When I parse '<payload>' with padding <padding> as a Position Report Extended Class B
    Then NmeaAisPositionReportExtendedClassBParser.TrueHeadingDegrees is <trueHeading>

    Examples:
    | payload                                              | padding | trueHeading |
    | C000000000000000000000000000000000000000000000000000 | 0       | 0           |
    | C000000000000000000000P00000000000000000000000000000 | 0       | 1           |
    | C00000000000000000003wP00000000000000000000000000000 | 0       | 511         |

Scenario Outline: Time Stamp
    When I parse '<payload>' with padding <padding> as a Position Report Extended Class B
    Then NmeaAisPositionReportExtendedClassBParser.TimeStampSecond is <timeStamp>

    Examples:
    | payload                                              | padding | timeStamp |
    | C000000000000000000000000000000000000000000000000000 | 0       | 0         |
    | C0000000000000000000000P0000000000000000000000000000 | 0       | 1         |
    | C000000000000000000000MP0000000000000000000000000000 | 0       | 59        |

Scenario Outline: Regional Reserved bits 139-142
    When I parse '<payload>' with padding <padding> as a Position Report Extended Class B
    Then NmeaAisPositionReportExtendedClassBParser.RegionalReserved139 is <reserved>

    Examples:
    | payload                                              | padding | reserved |
    | C000000000000000000000000000000000000000000000000000 | 0       | 0        |
    | C000000000000000000000020000000000000000000000000000 | 0       | 1        |
    | C000000000000000000000040000000000000000000000000000 | 0       | 2        |
    | C000000000000000000000080000000000000000000000000000 | 0       | 4        |
    | C0000000000000000000000@0000000000000000000000000000 | 0       | 8        |
    | C0000000000000000000000H0000000000000000000000000000 | 0       | 12       |
    | C0000000000000000000000N0000000000000000000000000000 | 0       | 15       |

Scenario Outline: Name
    When I parse '<payload>' with padding <padding> as a Position Report Extended Class B
    Then NmeaAisPositionReportExtendedClassBParser.Name is '<name>'

    Examples:
    | payload                                              | padding | name                 |
    | C000000000000000000000000000000000000000000000000000 | 0       | @@@@@@@@@@@@@@@@@@@@ |
    | C000000000000000000000002000000000000000000000000000 | 0       | A@@@@@@@@@@@@@@@@@@@ |
    | C000000000000000000000002468:<>@BDFHJLNPRTV`00000000 | 0       | ABCDEFGHIJKLMNOPQRST |
    | C00000000000000000000000bdfhjlnprtw13579;=?A00000000 | 0       | UVWXYZ[\]^_ !"#$%&'( |
    | C00000000000000000000001CEGIKMOQSUWacegikmoq00000000 | 0       | )*+,-./0123456789:;< |
    | C00000000000000000000001suv0000000000000000000000000 | 0       | =>?@@@@@@@@@@@@@@@@@ |

Scenario Outline: Ship type
    When I parse '<payload>' with padding <padding> as a Position Report Extended Class B
    Then NmeaAisPositionReportExtendedClassBParser.ShipType is <type>

    Examples:
    | payload                                              | padding | type                           |
    | C000000000000000000000000000000000000000000000000000 | 0       | NotAvailable                   |
    | C0000000000000000000000000000000000000000000:P000000 | 0       | WingInGroundHazardousCategoryA |

Scenario Outline: Dimension to Bow
    When I parse '<payload>' with padding <padding> as a Position Report Extended Class B
    Then NmeaAisPositionReportExtendedClassBParser.DimensionToBow is <size>

    Examples:
    | payload                                              | padding | size |
    | C000000000000000000000000000000000000000000000000000 | 0       | 0    |
    | C000000000000000000000000000000000000000000000400000 | 0       | 1    |
    | C00000000000000000000000000000000000000000000Ot00000 | 0       | 511  |

Scenario Outline: Dimension to Stern
    When I parse '<payload>' with padding <padding> as a Position Report Extended Class B
    Then NmeaAisPositionReportExtendedClassBParser.DimensionToStern is <size>

    Examples:
    | payload                                              | padding | size |
    | C000000000000000000000000000000000000000000000000000 | 0       | 0    |
    | C00000000000000000000000000000000000000000000000P000 | 0       | 1    |
    | C0000000000000000000000000000000000000000000003wP000 | 0       | 511  |

Scenario Outline: Dimension to Port
    When I parse '<payload>' with padding <padding> as a Position Report Extended Class B
    Then NmeaAisPositionReportExtendedClassBParser.DimensionToPort is <size>

    Examples:
    | payload                                              | padding | size |
    | C000000000000000000000000000000000000000000000000000 | 0       | 0    |
    | C000000000000000000000000000000000000000000000000P00 | 0       | 1    |
    | C00000000000000000000000000000000000000000000000OP00 | 0       | 63   |

Scenario Outline: Dimension to Starboard
    When I parse '<payload>' with padding <padding> as a Position Report Extended Class B
    Then NmeaAisPositionReportExtendedClassBParser.DimensionToStarboard is <size>

    Examples:
    | payload                                              | padding | size |
    | C000000000000000000000000000000000000000000000000000 | 0       | 0    |
    | C0000000000000000000000000000000000000000000000000P0 | 0       | 1    |
    | C000000000000000000000000000000000000000000000000OP0 | 0       | 63   |

Scenario Outline: Position fix type
    When I parse '<payload>' with padding <padding> as a Position Report Extended Class B
    Then NmeaAisPositionReportExtendedClassBParser.PositionFixType is <epfd>

    Examples:
    | payload                                              | padding | epfd                       |
    | C000000000000000000000000000000000000000000000000000 | 0       | Undefined                  |
    | C000000000000000000000000000000000000000000000000020 | 0       | Gps                        |
    | C000000000000000000000000000000000000000000000000040 | 0       | Glonass                    |
    | C000000000000000000000000000000000000000000000000060 | 0       | CombinedGpsGlonass         |
    | C000000000000000000000000000000000000000000000000080 | 0       | LoranC                     |
    | C0000000000000000000000000000000000000000000000000:0 | 0       | Chayka                     |
    | C0000000000000000000000000000000000000000000000000<0 | 0       | IntegratedNavigationSystem |
    | C0000000000000000000000000000000000000000000000000>0 | 0       | Surveyed                   |
    | C0000000000000000000000000000000000000000000000000@0 | 0       | Galileo                    |

Scenario Outline: RAIM flag
    When I parse '<payload>' with padding <padding> as a Position Report Extended Class B
    Then NmeaAisPositionReportExtendedClassBParser.RaimFlag is <raim>

    Examples:
    | payload                                              | padding | raim  |
    | C000000000000000000000000000000000000000000000000000 | 0       | false |
    | C000000000000000000000000000000000000000000000000010 | 0       | true  |

Scenario Outline: DTE ready
    When I parse '<payload>' with padding <padding> as a Position Report Extended Class B
    Then NmeaAisPositionReportExtendedClassBParser.DteNotReady is <notReady>

    Examples:
    | payload                                              | padding | notReady |
    | C000000000000000000000000000000000000000000000000000 | 0       | false    |
    | C00000000000000000000000000000000000000000000000000P | 0       | true     |

Scenario Outline: Assigned flag
    When I parse '<payload>' with padding <padding> as a Position Report Extended Class B
    Then NmeaAisPositionReportExtendedClassBParser.IsAssigned is <isAssigned>

    Examples:
    | payload                                              | padding | isAssigned |
    | C000000000000000000000000000000000000000000000000000 | 0       | false      |
    | C00000000000000000000000000000000000000000000000000@ | 0       | true       |

Scenario Outline: Spare
    When I parse '<payload>' with padding <padding> as a Position Report Extended Class B
    Then NmeaAisPositionReportExtendedClassBParser.Spare308 is <spare>

    Examples:
    | payload                                              | padding | spare |
    | C000000000000000000000000000000000000000000000000000 | 0       | 0     |
    | C000000000000000000000000000000000000000000000000001 | 0       | 1     |
    | C000000000000000000000000000000000000000000000000002 | 0       | 2     |
    | C000000000000000000000000000000000000000000000000007 | 0       | 7     |
    | C000000000000000000000000000000000000000000000000008 | 0       | 8     |
    | C00000000000000000000000000000000000000000000000000> | 0       | 14    |
    | C00000000000000000000000000000000000000000000000000? | 0       | 15    |
