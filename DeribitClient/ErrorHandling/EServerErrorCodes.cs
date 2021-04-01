﻿namespace DeribitClient.Validator
{
    public enum EServerErrorCodes
    {
        NO_ERROR = 0,
        API_NOT_ENABLED = 9999,
        AUTHORIZATION_REQUIRED = 10000,
        ERROR = 10001,
        QTY_TOO_LOW = 10002,
        ORDER_OVERLAP = 10003,
        ORDER_NOT_FOUND = 10004,
        PRICE_TOO_LOW = 10005,
        PRICE_TOO_LOW4IDX = 10006,
        PRICE_TOO_HIGH = 10007,
        PRICE_TOO_HIGH4IDX = 10008,
        NOT_ENOUGH_FUNDS = 10009,
        ALREADY_CLOSED = 10010,
        PRICE_NOT_ALLOWED = 10011,
        BOOK_CLOSED = 10012,
        PME_MAX_TOTAL_OPEN_ORDERS = 10013,
        PME_MAX_FUTURE_OPEN_ORDERS = 10014,
        PME_MAX_OPTION_OPEN_ORDERS = 10015,
        PME_MAX_FUTURE_OPEN_ORDERS_SIZE = 10016,
        PME_MAX_OPTION_OPEN_ORDERS_SIZE = 10017,
        NON_PME_MAX_FUTURE_POSITION_SIZE = 10018,
        LOCKED_BY_ADMIN = 10019,
        INVALID_OR_UNSUPPORTED_INSTRUMENT = 10020,
        INVALID_AMOUNT = 10021,
        INVALID_QUANTITY = 10022,
        INVALID_PRICE = 10023,
        INVALID_MAX_SHOW = 10024,
        INVALID_ORDER_ID = 10025,
        PRICE_PRECISION_EXCEEDED = 10026,
        NON_INTEGER_CONTRACT_AMOUNT = 10027,
        TOO_MANY_REQUESTS = 10028,
        NOT_OWNER_OF_ORDER = 10029,
        MUST_BE_WEBSOCKET_REQUEST = 10030,
        INVALID_ARGS_FOR_INSTRUMENT = 10031,
        WHOLE_COST_TOO_LOW = 10032,
        NOT_IMPLEMENTED = 10033,
        STOP_PRICE_TOO_HIGH = 10034,
        STOP_PRICE_TOO_LOW = 10035,
        INVALID_MAX_SHOW_AMOUNT = 10036,
        RETRY = 10040,
        SETTLEMENT_IN_PROGRESS = 10041,
        PRICE_WRONG_TICK = 10043,
        STOP_PRICE_WRONG_TICK = 10044,
        CAN_NOT_CANCEL_LIQUIDATION_ORDER = 10045,
        CAN_NOT_EDIT_LIQUIDATION_ORDER = 10046,
        MATCHING_ENGINE_QUEUE_FULL = 10047,
        NOT_ON_THIS_SERVER = 10048,
        CANCEL_ON_DISCONNECT_FAILED = 10049,
        TOO_MANY_CONCURRENT_REQUESTS = 10066,
        ALREADY_FILLED = 11008,
        INVALID_ARGUMENTS = 11029,
        OTHER_REJECT = 11030,
        OTHER_ERROR = 11031,
        NO_MORE_STOPS = 11035,
        INVALID_STOP_PRICE = 11036,
        OUTDATED_INSTRUMENT_FOR_IV_ORDER = 11037,
        NO_ADV_FOR_FUTURES = 11038,
        NO_ADV_POSTONLY = 11039,
        NOT_ADV_ORDER =  11041,
        PERMISSION_DENIED = 11042,
        BAD_ARGUMENT = 11043,
        NOT_OPEN_ORDER = 11044,
        INVALID_EVENT = 11045,
        OUTDATED_INSTRUMENT = 11046,
        UNSUPPORTED_ARG_COMBINATION = 11047,
        WRONG_MAX_SHOW_FOR_OPTION = 11048,
        BAD_ARGUMENTS = 11049,
        BAD_REQUEST = 11050,
        SYSTEM_MAINTANENCE = 11051,
        SUBSCRIBE_ERROR_UNSUBSCRIBED = 11052,
        TRANSFER_NOT_FOUND = 11053,
        INVALID_ADDR = 11090,
        INVALID_TRANSFER_ADDRESS = 11091,
        ADDRESS_ALREADY_EXISTS = 11092,
        MAX_ADDR_COUNT_EXCEEDED = 11093,
        INTERNAL_SERVER_ERROR = 11094,
        DISABLED_DEPOSIT_ADDRESS_CREATION = 11095,
        ADDRESS_BELONGS_TO_USER = 11096,
        BAD_TFA = 12000,
        TOO_MANY_SUBACCOUNTS = 12001,
        WRONG_SUBACCOUNT_NAME = 12002,
        TFA_OVER_LIMIT = 12998,
        LOGIN_OVER_LIMIT = 12003,
        REGISTRATION_OVER_LIMIT = 12004,
        COUNTRY_IS_BANNED = 12005,
        TRANSFER_IS_NOT_ALLOWED = 12100,
        TFA_USED = 12999,
        INVALID_LOGIN = 13000,
        ACCOUNT_NOT_ACTIVATED = 13001,
        ACCOUNT_BLOCKED = 13002,
        TFA_REQUIRED = 13003,
        INVALID_CREDENTIALS = 13004,
        PWD_MATCH_ERROR = 13005,
        SECURITY_ERROR = 13006,
        USER_NOT_FOUND = 13007,
        REQUEST_FAILED = 13008,
        UNAUTHORIZED = 13009,
        VALUE_REQUIRED = 13010,
        VALUE_TOO_SHORT = 13011,
        UNAVAILABLE_IN_SUBACCOUNT = 13012,
        INVALID_PHONE_NUMBER = 13013,
        CANNOT_SEND_SMS = 13014,
        INVALID_SMS_CODE = 13015,
        INVALID_INPUT = 13016,
        SUBSCRIPTION_FAILED = 13017,
        INVALID_CONTENT_TYPES = 13018,
        ORDERBOOK_CLOSED = 13019,
        NOT_FOUND = 13020,
        FORBIDDEN = 13021,
        METHOD_SWITCHED_OFF_BY_ADMIN = 13025,
        VERIFICATION_REQUIRED = 13031,
        UNAVAILABLE = 13503,
        REQUEST_CANCELLED_BY_USER = 13666,
        REPLACED = 13777,
        INVALID_PARAMS = -32602,
        METHOD_NOT_FOUND = -32601,
        PARSE_ERROR = -32700,
        MISSING_PARAMS = -32000
    }
}