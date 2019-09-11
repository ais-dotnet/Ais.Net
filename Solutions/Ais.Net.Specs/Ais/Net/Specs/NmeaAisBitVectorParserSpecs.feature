# Copyright (c) Endjin Limited. All rights reserved.
#
# Contains data under the Norwegian licence for Open Government data (NLOD) distributed by
# the Norwegian Costal Administration - https://ais.kystverket.no/
# The license can be found at https://data.norge.no/nlod/en/2.0
# The lines in this file that contain data from this source are annotated with a comment containing "ais.kystverket.no"
# The NLOD applies only to the data in these annotated lines. The license under which you are using this software
# (either the AGPLv3, or a commercial license) applies to the whole file.

Feature: NmeaAisBitVectorParserSpecs
    In order to process the AIS data in the payload of an NMEA message
    As a developer writing parsers to process specific AIS messages
    I want to be able to extract arbitrary bit ranges from data encoded with the 'AIVDM/AIVDO Payload Armoring'

Scenario Outline: Unsigned aligned 6-bit integer
    Given an NMEA AIS payload of '<payload>' and padding <padding>
    When I read an unsigned 6 bit int at offset <offset>
    Then the NmeaAisBitVectorParser returns an unsigned integer with value <result>

    Examples: Message type field at start of message
        | payload                                                      | padding | offset | result |
        | 1000000000000000000000000000                                 | 0       | 0      | 1      |
        | 13nW5<00000IoPlSbE`:P8EH0534                                 | 0       | 0      | 1      |    # ais.kystverket.no
        | 2000000000000000000000000000                                 | 0       | 0      | 2      |
        | 24c`1`001kPEGSLR98IP00462D0s                                 | 0       | 0      | 2      |    # ais.kystverket.no
        | 3000000000000000000000000000                                 | 0       | 0      | 3      |
        | 33n24L00000p3bHUiw<v46`60?Kk                                 | 0       | 0      | 3      |    # ais.kystverket.no
        | 50000000000000000000000000000000000000000000000000000000     | 0       | 0      | 5      |
        | 53m89J81SR44hdhL0004eH5A84q<00000000001@4P8825LD08j0DTm0A000 | 0       | 0      | 5      |    # ais.kystverket.no
        | B000000000000000000000000000                                 | 0       | 0      | 18     |
        | B3q35T005h<0h@`Dd:i;gwRUoP06                                 | 0       | 0      | 18     |    # ais.kystverket.no
        | H00000000000000000000000000                                  | 2       | 0      | 24     |
        | H3mhO30PDT@V04pU@4000000000                                  | 2       | 0      | 24     |    # ais.kystverket.no

    Examples: Minutes and seconds in base station report
        | payload                      | padding | offset | result |
        | 40000000000M0000000000000000 | 0       | 66     | 29     |
        | 400000000000c000000000000000 | 0       | 72     | 43     |
        | 40000000000O0000000000000000 | 0       | 66     | 31     |
        | 400000000000T000000000000000 | 0       | 72     | 36     |

    Examples: End of data
        | payload | padding | offset | result |
        | ABC0    | 0       | 18     | 0      |
        | ABC1    | 0       | 18     | 1      |
        | ABC:    | 0       | 18     | 10     |
        | ABCA    | 0       | 18     | 17     |
        | ABC`    | 0       | 18     | 40     |
        | ABCa    | 0       | 18     | 41     |
        | ABCw    | 0       | 18     | 63     |

Scenario Outline: Unsigned misaligned 6-bit integer
    Given an NMEA AIS payload of '<payload>' and padding <padding>
    When I read an unsigned 6 bit int at offset <offset>
    Then the NmeaAisBitVectorParser returns an unsigned integer with value <result>

    Examples: Time Stamp in Common Navigation Block
        | payload                      | padding | offset | result |
        | 1000000000000000000000020000 | 0       | 137    | 1      |
        | 1000000000000000000000040000 | 0       | 137    | 2      |
        | 1000000000000000000000060000 | 0       | 137    | 3      |
        | 1000000000000000000000100000 | 0       | 137    | 32     |
        | 1000000000000000000000120000 | 0       | 137    | 33     |
        | 10000000000000000000001d0000 | 0       | 137    | 54     |
        | 10000000000000000000001f0000 | 0       | 137    | 55     |
        | 13nW5<00000IoPlSbE`:P8EH0534 | 0       | 137    | 44     |    # ais.kystverket.no
        | 13oO7800000FH>>S6rpur`P200S< | 0       | 137    | 1      |    # ais.kystverket.no

    Examples: Functional ID in Binary Addressed Message
        | payload                                         | padding | offset | result |
        | 00000000000000000000000000000000000000000000000 | 2       | 82     | 0      |
        | 00000000000000400000000000000000000000000000000 | 2       | 82     | 1      |
        | 00000000000000800000000000000000000000000000000 | 2       | 82     | 2      |
        | 00000000000001000000000000000000000000000000000 | 2       | 82     | 16     |
        | 00000000000001400000000000000000000000000000000 | 2       | 82     | 17     |
        | 00000000000002000000000000000000000000000000000 | 2       | 82     | 32     |
        | 00000000000002H00000000000000000000000000000000 | 2       | 82     | 38     |
        | 00000000000003t00000000000000000000000000000000 | 2       | 82     | 63     |

    #    e      o      g      f
    # 101101 110111 101111 101110
    #  01101 1  - 27
    #   1101 11  - 55
    #    101 110  - 46
    #     01 1101  - 29
    #      1 11011  - 59
    #         10111 1  - 47
    #          0111 10  - 30
    #           111 101  - 61
    #            11 1011  - 59
    #             1 10111  - 55
    #                01111 1  - 31
    #                 1111 10  - 62
    #                  111 101  - 61
    #                   11 1011  - 59
    #                    1 10111  - 55
    Examples: Synthetic examples testing at various bit positions
        | payload | padding | offset | result |
        | eogf    | 1       | 1      | 27     |
        | eogf    | 1       | 2      | 55     |
        | eogf    | 1       | 3      | 46     |
        | eogf    | 1       | 4      | 29     |
        | eogf    | 1       | 5      | 59     |
        | eogf    | 1       | 7      | 47     |
        | eogf    | 1       | 8      | 30     |
        | eogf    | 1       | 9      | 61     |
        | eogf    | 1       | 10     | 59     |
        | eogf    | 1       | 11     | 55     |
        | eogf    | 1       | 13     | 31     |
        | eogf    | 1       | 14     | 62     |
        | eogf    | 1       | 15     | 61     |
        | eogf    | 1       | 16     | 59     |
        | eogf    | 1       | 17     | 55     |

Scenario Outline: Signed 8-bit integer
    Given an NMEA AIS payload of '<payload>' and padding <padding>
    When I read a signed 8 bit int at offset <offset>
    Then the NmeaAisBitVectorParser returns an signed integer with value <result>

    Examples: Rate of Turn in Common Navigation Block
        | payload                      | padding | offset | result |
        | 0000000000000000000000000000 | 0       | 42     | 0      |
        | 00000000@0000000000000000000 | 0       | 42     | 1      |
        | 00000000h0000000000000000000 | 0       | 42     | 3      |
        | 0000000100000000000000000000 | 0       | 42     | 4      |
        | 00000001@0000000000000000000 | 0       | 42     | 5      |
        | 0000000Oh0000000000000000000 | 0       | 42     | 127    |
        | 0000000P00000000000000000000 | 0       | 42     | -128   |
        | 13oO7800000FH>>S6rpur`P200S< | 0       | 42     | 0      |    # ais.kystverket.no
        | 13oHtV7OhN0=B9bQch;WqnCp0W3h | 0       | 42     | 127    |    # ais.kystverket.no
        | 13mCIp0P00PFnJBSHS1>4?wH2@JB | 0       | 42     | -128   |    # ais.kystverket.no

    #    e      o      g      f
    # 101101 110111 101111 101110
    # 101101 11  -73
    #  01101 110  110
    #   1101 1101  -35
    #    101 11011  -69
    #     01 110111  119
    #      1 110111 1  -17
    #        110111 10  -34
    Examples: Synthetic examples testing at various bit positions
        | payload | padding | offset | result |
        | eogf    | 1       | 0      | -73    |
        | eogf    | 1       | 1      | 110    |
        | eogf    | 1       | 2      | -35    |
        | eogf    | 1       | 3      | -69    |
        | eogf    | 1       | 4      | 119    |
        | eogf    | 1       | 5      | -17    |
        | eogf    | 1       | 6      | -34    |


Scenario Outline: Single bit
    Given an NMEA AIS payload of '<payload>' and padding <padding>
    When I read an unsigned 1 bit int at offset <offset>
    Then the NmeaAisBitVectorParser returns an unsigned integer with value <result>

    Examples: Position Accuracy in Common Navigation Block
        | payload                      | padding | offset | result |
        | 0000                         | 0       | 0      | 0      |
        | 1000                         | 0       | 0      | 0      |
        | 1000                         | 0       | 1      | 0      |
        | 1000                         | 0       | 2      | 0      |
        | 1000                         | 0       | 3      | 0      |
        | 1000                         | 0       | 4      | 0      |
        | 1000                         | 0       | 5      | 1      |
        | 2000                         | 0       | 3      | 0      |
        | 2000                         | 0       | 4      | 1      |
        | 2000                         | 0       | 5      | 0      |
        | 0000000000P                  | 0       | 59     | 0      |
        | 0000000000P                  | 0       | 60     | 1      |
        | 0000000000P                  | 0       | 61     | 0      |
        | 0000000000000000000000000000 | 0       | 60     | 0      |
        | 0000000000P00000000000000000 | 0       | 60     | 1      |
        | 13mCIp0P00PFnJBSHS1>4?wH2@JB | 0       | 60     | 1      |    # ais.kystverket.no
        | 13nW5<00000IoPlSbE`:P8EH0534 | 0       | 60     | 0      |    # ais.kystverket.no