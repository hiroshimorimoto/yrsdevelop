export default class DateUtil {
    constructor() {
    }

    static ToDateArray(strDateArray) {
        var ret = [];
        for (var d of strDateArray) {
            ret.push(new Date(d));
        }
        return ret;
    }

    static GetDayOfWeekChar(dateTime) {
        if (!dateTime) return "";
        if (!dateTime.getDay) dateTime = new Date(dateTime);

        return ["日", "月", "火", "水", "木", "金", "土"][dateTime.getDay()];
    }


    static GetDateString(datetime) {

        if (!datetime) return "";

        try {
            var d = new Date(datetime);

            var formatted = `${d.getFullYear()}/${d.getMonth() + 1}/${d.getDate()}`
                .replace(/\n|\r/g, '');

            return formatted;
        } catch (error) {
            return datetime;
        }
    }


    static GetDateStringZero(datetime) {
        if (!datetime) return "";

        try {
            var d = new Date(datetime);
            var formatted = `${d.getFullYear()}/${(d.getMonth() + 1).toString().padStart(2, '0')}/${d.getDate().toString().padStart(2, '0')}`
                .replace(/\n|\r/g, '');

            return formatted;
        } catch (error) {
            return datetime;
        }
    }

    static GetDateStringZeroJA(datetime) {
        if (!datetime) return "";

        try {
            var d = new Date(datetime);
            var formatted = `${d.getFullYear()}年${(d.getMonth() + 1).toString().padStart(2, '0')}月${d.getDate().toString().padStart(2, '0')}日`
                .replace(/\n|\r/g, '');

            return formatted;
        } catch (error) {
            return datetime;
        }

    }


    static GetDateStringZeroJA_WithTime(datetime) {
        if (!datetime) return "";

        try {
            var d = new Date(datetime);
            var formatted = `${d.getFullYear()}年${(d.getMonth() + 1).toString().padStart(2, '0')}月${d.getDate().toString().padStart(2, '0')}日 ${d.getHours().toString().padStart(2, '0')}時${d.getMinutes().toString().padStart(2, '0')}分`
                .replace(/\n|\r/g, '');

            return formatted;
        } catch (error) {
            return datetime;
        }

    }



    static SimpleCheckDateYMD(strDate) {
        //未入力時はチェックなし
        if (!strDate) return false;

        if (!strDate.match(/^\d{4}\/\d{2}\/\d{2}$/)) {
            return false;
        }

        var y = strDate.split("/")[0];
        var m = strDate.split("/")[1] - 1;
        var d = strDate.split("/")[2];
        var date = new Date(y, m, d);

        if (date.getFullYear() != y || date.getMonth() != m || date.getDate() != d) {
            return false;
        }
        return true;
    }

    /****************************************************************
    * 機　能： 入力された値が日付でYYYY/MM/DD形式になっているか調べる
    * 引　数： strDate = 入力された値
    * 戻り値： 正 = true　不正 = false
    ****************************************************************/
    static checkDateYMD(strDate) {
        //未入力時はチェックなし
        if (!strDate) return false;

        if (!strDate.match(/^\d{4}\/\d{2}\/\d{2}$/)) {
            return false;
        }

        strDate = DateUtil.GetDateStringZero(strDate);

        var y = strDate.split("/")[0];
        var m = strDate.split("/")[1] - 1;
        var d = strDate.split("/")[2];
        var date = new Date(y, m, d);
        if (date.getFullYear() != y || date.getMonth() != m || date.getDate() != d) {
            return false;
        }
        return true;
    }

    static GetDateValue(strDate) {
        //未入力時はチェックなし
        if (!strDate) return null;

        if (!strDate.match(/^\d{4}\/\d{2}\/\d{2}$/)) {
            return null;
        }
        var y = strDate.split("/")[0];
        var m = strDate.split("/")[1] - 1;
        var d = strDate.split("/")[2];
        var date = new Date(y, m, d);
        if (date.getFullYear() != y || date.getMonth() != m || date.getDate() != d) {
            return null;
        }
        return date;
    }

    static GetAge(birthDate) {
        var today = new Date();
        birthDate = GetDateValue(GetDateValue);
        var now = today.getFullYear() * 10000 + (today.getMonth() + 1) * 100 + today.getDate();
        var bd = birthDate.getFullYear() * 10000 + (birthDate.getMonth() + 1) * 100 + birthDate.getDate();
        return (Math.floor((now - bd) / 10000));
    }
}