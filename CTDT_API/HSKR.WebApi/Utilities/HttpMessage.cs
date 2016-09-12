using System;

namespace CTDT.WebApi.Utilities
{
	public static class HttpMessage
	{
        public static readonly string ERROR_GET_DATA = " Có lỗi khi xử lý";
        public static readonly string INVALID_MODEL = "Invalid model";
        public static readonly string ERROR_CREATE = " Error when Create";
        public static readonly string NOT_FOUND_ALERTISSUE = "この項目の関連情報が習得できなかったです。もう一度やり直してください。もしできない場合は技術者にご報告ください。";
        public static readonly string ERROR_EDIT = "Error when edit";
        public static readonly string EXIST_COLOR = "既存カーラーコードなので、新作できません。もう一度やり直してください。もしできない場合は技術者にご報告ください。";
        public static readonly string NOT_EXIST_COLOR = "カーラーコードが存在していません。ご確認の上、もう一度やり直してください。もしできない場合は技術者にご報告ください。";

        public static readonly string COLOR_USED = "が使われているカーラーのため、削除できません。";

        public static readonly string ERROR_DELETE = "Error when delete";
        public static readonly string EMPTY_DATA = "送信されたデーターが空なので、もう一回お確かめの上、おやり直しください。もしできない場合は技術者にご報告ください。";
        public static readonly string NOT_FOUND_COMPANY = "指定された会社の情報が取得できなかったです。もう一回お確かめの上、おやり直しください。もしできない場合は技術者にご報告ください。";
        public static readonly string ERROR_GET_COUNTRY = "指定された国の情報が取得できなかったです。もう一回お確かめの上、おやり直しください。もしできない場合は技術者にご報告ください。";
        public static readonly string EXIST_COUNTRY = "追加された国なので、新作できません。もう一度やり直してください。もしできない場合は技術者にご報告ください。";
        public static readonly string NOT_FOUND_COUNTRY = "指定された国の情報が取得できなかったです。もう一回お確かめの上、おやり直しください。もしできない場合は技術者にご報告ください。";
        public static readonly string EXIST_CUSTOMER = "追加されたカスタマーなので、新作できません。もう一度やり直してください。もしできない場合は技術者にご報告ください。";
        public static readonly string NOT_FOUND_CUSTOMER = "指定されたカスタマーの情報が取得できなかったです。もう一回お確かめの上、おやり直しください。もしできない場合は技術者にご報告ください。";
        public static readonly string EXIST_MATERIAL = "追加された資材コードなので、新作できません。もう一度やり直してください。もしできない場合は技術者にご報告ください。";
        public static readonly string NOT_EXIST_MATERIAL = "指定された資材コードの情報が取得できなかったです。もう一回お確かめの上、おやり直しください。もしできない場合は技術者にご報告ください。";
        public static readonly string OVER_QUANTITY = "残数以下の数量を設定してください。";
        public static readonly string NOT_EXIST_PRODUCT_PATH_1 = "指定された項目 ";
        public static readonly string NOT_EXIST_PRODUCT_PATH_2 = " の工程の情報が取得できなかったです。もう一回お確かめの上、おやり直しください。もしできない場合は技術者にご報告ください。";
        public static readonly string ERROR_GET_MAX_PRIORITY = "最大優先度の情報が取得できなかったです。もう一回お確かめの上、おやり直しください。もしできない場合は技術者にご報告ください。";
        public static readonly string DATA_NOT_FOUND = "情報が存在していません。もう一回お確かめの上、おやり直しください。もしできない場合は技術者にご報告ください。";
        public static readonly string NOT_EXIST_EMPLOYEE = "指定された資材の情報が存在していません。もう一回お確かめの上、おやり直しください。もしできない場合は技術者にご報告ください。";
        public static readonly string ERROR_IMG_TYPE = "サポートされているイメージタイプをアップロードください。（.jpg,.gif,.png） ";
        public static readonly string ERROR_IMG_SIZE_3 = "3MB以下のファイルをアップロードください。";
        public static readonly string ERROR_IMG_SIZE_5 = "File > 10Mb ";
        public static readonly string NOT_EXIST_TASK_EMPLOYEE = "タスクIDと社員IDが存在していますせん。もう一回お確かめの上、おやり直しください。もしできない場合は技術者にご報告ください。";
        public static readonly string ERROR_IMG_NULL = "イメージをアップロードください。（.jpg,.gif,.png） のタイプでお願いします。";
        //public static readonly string NOT_EXIST_EMPLOYEE = "指定された作業者（xxx）が存在していません。もう一回お確かめの上、おやり直しください。もしできない場合は技術者にご報告ください。";
        public static readonly string EXIST_ORDER = "既存品番なので、新作できません。もう一度やり直してください。もしできない場合は技術者にご報告ください。";
        public static readonly string NOT_FOUND_ORDER = "品番が見付かりませんでした。もう一回お確かめの上、おやり直しください。もしできない場合は技術者にご報告ください。";
        public static readonly string NULL_STRJSON = "strJsonがnullです。ウェブブラウザーをリスタートしてください。もしできない場合は技術者にご報告ください。";
        public static readonly string EXIST_PATH = "既存工程名なので、新作できません。もう一度やり直してください。もしできない場合は技術者にご報告ください。";
        public static readonly string NOT_FOUND_PATH = "工程名が見付かりませんでした。もう一度やり直してください。もしできない場合は技術者にご報告ください。";
        public static readonly string NOT_FOUND_PRODUCT = "項目が見付かりませんでした。もう一度やり直してください。もしできない場合は技術者にご報告ください。";
        public static readonly string NOT_FOUND_PRODUCTPATH = "工程名が見付かりませんでした。もう一度やり直してください。もしできない場合は技術者にご報告ください。";
        public static readonly string NOT_EXIST_REQUESTTASK = "指定されたタスクID（）が存在していません。もう一回お確かめの上、おやり直しください。もしできない場合は技術者にご報告ください。";
        public static readonly string NOT_EXIST_STYLE = "指定されたスタイルID（）が存在していません。もう一回お確かめの上、おやり直しください。もしできない場合は技術者にご報告ください。";
        public static readonly string EXIST_STYLE_1 = "既存スタイルID ";
        public static readonly string EXIST_STYLE_2 = " なので、新作できません。もう一度やり直してください。もしできない場合は技術者にご報告ください。";
        public static readonly string EXIST_SUPPLY_1 = "既存スタイルID ";
        public static readonly string EXIST_SUPPLY_2 = " なので、新作できません。もう一度やり直してください。もしできない場合は技術者にご報告ください。";
        public static readonly string NOT_EXIST_SUPPLY = "指定されたプロバイダー（）が存在していません。もう一回お確かめの上、おやり直しください。もしできない場合は技術者にご報告ください。";
        public static readonly string LIMIT_QUANTITY_1 = "残数 ";
        public static readonly string LIMIT_QUANTITY_2 = " 以下の数量を設定してください。";
        public static readonly string NOT_FOUND_TASK = "タスク情報が見付かりませんでした。もう一度やり直してください。もしできない場合は技術者にご報告ください。";
        public static readonly string NULL_PRODUCT_OR_EMPLOYEE = "項目IDか作業者IDがなかったです。もう一回お確かめの上、おやり直しください。もしできない場合は技術者にご報告ください。";
        public static readonly string LIMIT_PRIORITY_1 = "優先度が ";
        public static readonly string LIMIT_PRIORITY_2 = " の以下をご設定ください。";
        public static readonly string NOT_FOUND_TEAM = "グループ情報が見つかりませんでした。もう一度やり直してください。もしできない場合は技術者にご報告ください。";
        public static readonly string LOGIN_INCORECT = "ユーザ名かパスワードが間違いました。もう一度やり直してください。もしできない場合は技術者にご報告ください。";
        public static readonly string LOGIN_FAIL = "システムにアクセスが出来ませでした。もう一度やり直してください。もしできない場合は技術者にご報告ください。";
        public static readonly string CHANGE_PASS_OLD_PASS = "新しいパースワードと新しいパースワード（再確認）が統一されていません。もう一度やり直してください。";
        public static readonly string CHANGE_PASS_WRONG_ACC = "ユーザ名か現在のパスワードが間違いました。もう一度やり直してください。もしできない場合は技術者にご報告ください。";
        public static readonly string ERROR_CHANGE_PASS = "パースワード変更に問題が発生しました。もう一度やり直してください。もしできない場合は技術者にご報告ください。";
        public static readonly string ERROR_SIGN_OUT = "ローグアウトに問題が発生しました。もう一度やり直してください。もしできない場合は技術者にご報告ください。";

        public static readonly string ERROR_NOT_DELETE_PRODUCT = "ローグアウトに問題が発生しました。もう一度やり直してください。もしできない場合は技術者にご報告ください。";
        public static readonly string EMPLOYEE_TEAMNAME_AS_NULL = "チーム無し";        
    }
}