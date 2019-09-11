# Copyright (c) Endjin Limited. All rights reserved.

Feature: LongRangeAisBroadcastParserSpecs
    In order process AIS messages from an nm4 file
    As a developer
    I want the NmeaAisLongRangeAisBroadcastParser to be able to parse the payload section of message type 27: Long Range AIS Broadcast

Scenario: Message Type
    When I parse 'K>eq`d@000000000' with padding 0 as a Long Range Ais Broadcast
    Then NmeaAisLongRangeAisBroadcastParser.Type is 27

Scenario Outline: Repeat Indicator
    When I parse '<payload>' with padding <padding> as a Long Range Ais Broadcast
    Then NmeaAisLongRangeAisBroadcastParser.RepeatIndicator is <repeatCount>

    Examples:
    | payload          | padding | repeatCount |
    | K000000000000000 | 0       | 0           |
    | K@00000000000000 | 0       | 1           |
    | KP00000000000000 | 0       | 2           |
    | Kh00000000000000 | 0       | 3           |

Scenario Outline: MMSI
    When I parse '<payload>' with padding <padding> as a Long Range Ais Broadcast
    Then NmeaAisLongRangeAisBroadcastParser.Mmsi is <mmsi>

    Examples:
    | payload          | padding | mmsi      |
    | K000000000000000 | 0       | 0         |
    | K00000@000000000 | 0       | 1         |
    | K00000P000000000 | 0       | 2         |

Scenario Outline: Position Accuracy
    When I parse '<payload>' with padding <padding> as a Long Range Ais Broadcast
    Then NmeaAisLongRangeAisBroadcastParser.PositionAccuracy is <positionAccuracy>

    Examples:
    | payload          | padding | positionAccuracy |
    | K000000000000000 | 0       | false            |
    | K000008000000000 | 0       | true             |

Scenario Outline: Raim Flag
    When I parse '<payload>' with padding <padding> as a Long Range Ais Broadcast
    Then NmeaAisLongRangeAisBroadcastParser.RaimFlag is <flag>

    Examples:
    | payload          | padding | flag  |
    | K000000000000000 | 0       | false |
    | K000004000000000 | 0       | true  |

Scenario Outline: Navigation Status
    When I parse '<payload>' with padding <padding> as a Long Range Ais Broadcast
    Then NmeaAisLongRangeAisBroadcastParser.NavigationStatus is <navigationStatus>

    Examples:
    | payload          | padding | navigationStatus           |
    | K000000000000000 | 0       | UnderwayUsingEngine        |
    | K000000@00000000 | 0       | AtAnchor                   |
    | K000000P00000000 | 0       | NotUnderCommand            |
    | K000000h00000000 | 0       | RestrictedManoeuverability |
    | K000001000000000 | 0       | ConstrainedByHerDraught    |
    | K000001@00000000 | 0       | Moored                     |
    | K000002000000000 | 0       | UnderWaySailing            |
    | K000003P00000000 | 0       | AisSartIsActive            |

Scenario Outline: Longitude and Latitute
    When I parse '<payload>' with padding <padding> as a Long Range Ais Broadcast
    Then NmeaAisLongRangeAisBroadcastParser.Longitude10thMins is <longitude>
    Then NmeaAisLongRangeAisBroadcastParser.Latitude10thMins is <latitude>

    Examples:
    | payload          | padding | longitude | latitude |
    | K000000000000000 | 0       | 0         | 0        |
    | K000000000@00000 | 0       | 1         | 0        |
    | K000000000000P00 | 0       | 0         | 1        |
    | K000000?wwh00000 | 0       | -1        | 0        |
    | K000000000?wwP00 | 0       | 0         | -1       |
    | K000000000000000 | 0       | 0         | 0        |

Scenario Outline: Speed Over Ground
    When I parse '<payload>' with padding <padding> as a Long Range Ais Broadcast
    Then NmeaAisLongRangeAisBroadcastParser.SpeedOverGroundTenths is <speedOverGround>

    Examples:
    | payload          | padding | speedOverGround |
    | K000000000000000 | 0       | 0               |
    | K0000000000000P0 | 0       | 1               |
    | K000000000000OP0 | 0       | 63              |

Scenario Outline: Course Over Ground
    When I parse '<payload>' with padding <padding> as a Long Range Ais Broadcast
    Then NmeaAisLongRangeAisBroadcastParser.CourseOverGroundDegrees is <courseOverGround>

    Examples:
    | payload          | padding | courseOverGround |
    | K000000000000000 | 0       | 0                |
    | K000000000000004 | 0       | 1                |
    | K000000000000010 | 0       | 16               |
    | K0000000000000FL | 0       | 359              |
    | K0000000000000Ot | 0       | 511              |

Scenario Outline: Gnss Position Status
    When I parse '<payload>' with padding <padding> as a Long Range Ais Broadcast
    Then NmeaAisLongRangeAisBroadcastParser.NotGnssPosition is <gnssStatus>

    Examples:
    | payload          | padding | gnssStatus |
    | K000000000000000 | 0       | false      |
    | K000000000000002 | 0       | true       |

Scenario Outline: Spare
    When I parse '<payload>' with padding <padding> as a Long Range Ais Broadcast
    Then NmeaAisLongRangeAisBroadcastParser.Spare95 is <flag>

    Examples:
    | payload          | padding | flag  |
    | K000000000000000 | 0       | false |
    | K000000000000001 | 0       | true  |
