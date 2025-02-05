from email_validator import validate_email, EmailNotValidError


def is_email_format_correct(email: str) -> bool:
    assert isinstance(email, str)
    try:
        validate_email(email, check_deliverability=False)

    except EmailNotValidError as e:
        return False

    return True


def convert_to_key_value(request: str) -> dict:
    assert isinstance(request, str)
    command_parts = request.replace("\\final\\", "").lstrip("\\").split("\\")

    parts = [part for part in command_parts if part != "final"]
    dict = {}
    try:
        for i in range(0, len(parts), 2):
            if parts[i] not in dict:
                dict[parts[i].lower()] = parts[i + 1]
            # Some game send uppercase key to us, so we have to deal with it
    except IndexError:
        pass
    return dict


def is_valid_date(day: int, month: int, year: int) -> bool:
    # Check for a blank.
    if (day, month, year) == (0, 0, 0):
        return False

    # Validate the day of the month.
    match month:
        case 0:
            # Can't specify a day without a month.
            if day != 0:
                return False
        case 1, 3, 5, 7, 8, 10, 12:
            # 31-day month.
            if day > 31:
                return False
        case 4, 6, 9, 11:
            # 30-day month.
            if day > 30:
                return False
        case 2:
            # 28/29-day month.
            # Leap year?
            if ((year % 4 == 0) and (year % 100 != 0)) or (year % 400 == 0):
                if day > 29:
                    return False
            else:
                if day > 28:
                    return False
        case _:
            # Invalid month.
            return False

    # Check that the date is in the valid range.
    if year < 1900 or year > 2079:
        return False
    elif year == 2079:
        match month:
            case 6 if day > 6:
                return False
            case _:
                if month > 6:
                    return False

    return True


